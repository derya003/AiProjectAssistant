using System.Text;

using AiProjectAssistant.Api.Data;
using AiProjectAssistant.Api.Options;
using AiProjectAssistant.Api.Repositories;
using AiProjectAssistant.Api.Repositories.Interfaces;
using AiProjectAssistant.Api.Services;
using AiProjectAssistant.Api.Services.Interfaces;
using AiProjectAssistant.Api.Services.Providers;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// ----------------------------------------------------
// VERİTABANI
// ----------------------------------------------------

builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString =
        builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException(
            "DefaultConnection bağlantı bilgisi bulunamadı.");

    options.UseSqlServer(connectionString);
});

// ----------------------------------------------------
// JWT AYARLARI
// ----------------------------------------------------

builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection("Jwt"));

var jwtOptions = builder.Configuration
    .GetSection("Jwt")
    .Get<JwtOptions>()
    ?? throw new InvalidOperationException(
        "JWT ayarları bulunamadı.");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtOptions.Issuer,
                ValidAudience = jwtOptions.Audience,

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtOptions.Key)),

                ClockSkew = TimeSpan.Zero
            };
    });

builder.Services.AddAuthorization();

// ----------------------------------------------------
// CONTROLLER VE OPENAPI
// ----------------------------------------------------

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// ----------------------------------------------------
// REPOSITORY KAYITLARI
// ----------------------------------------------------

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();

// ----------------------------------------------------
// AUTHENTICATION SERVİSLERİ
// ----------------------------------------------------

builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// ----------------------------------------------------
// AI AYARLARI
// ----------------------------------------------------

builder.Services.Configure<AiProviderOptions>(
    builder.Configuration.GetSection("AiProvider"));

builder.Services.Configure<ClaudeOptions>(
    builder.Configuration.GetSection("Claude"));

builder.Services.Configure<OpenAIOptions>(
    builder.Configuration.GetSection("OpenAI"));

// Dış API istekleri için HttpClient desteği
builder.Services.AddHttpClient();

// ----------------------------------------------------
// AI SERVİSLERİ
// ----------------------------------------------------

builder.Services.AddScoped<IAiService, AiService>();

// Kullanılabilecek AI servisleri
builder.Services.AddScoped<ClaudeService>();
builder.Services.AddScoped<OpenAIService>();

// appsettings.json içindeki Provider değerine göre
// kullanılacak AI servisini seçer.
builder.Services.AddScoped<IAiProvider>(serviceProvider =>
{
    var providerOptions = serviceProvider
        .GetRequiredService<IOptions<AiProviderOptions>>()
        .Value;

    return providerOptions.Provider
        .Trim()
        .ToLowerInvariant() switch
    {
        "claude" =>
            serviceProvider.GetRequiredService<ClaudeService>(),

        "openai" =>
            serviceProvider.GetRequiredService<OpenAIService>(),

        _ => throw new InvalidOperationException(
            $"Desteklenmeyen AI sağlayıcısı: " +
            $"{providerOptions.Provider}")
    };
});

// ----------------------------------------------------
// SWAGGER VE JWT AUTHORIZE AYARLARI
// ----------------------------------------------------

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description =
                "JWT token bilgisini giriniz. " +
                "Sadece token değerini yazmanız yeterlidir."
        });

    options.AddSecurityRequirement(document =>
        new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference(
                "Bearer",
                document)] = new List<string>()
        });
});

// ----------------------------------------------------
// UYGULAMA
// ----------------------------------------------------

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapGet("/", () => Results.Redirect("/swagger/index.html"))
   .ExcludeFromDescription();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();