using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using AiProjectAssistant.Api.Data;
using Microsoft.EntityFrameworkCore;
using AiProjectAssistant.Api.Repositories;
using AiProjectAssistant.Api.Repositories.Interfaces;
using AiProjectAssistant.Api.Options;
using AiProjectAssistant.Api.Services;
using AiProjectAssistant.Api.Services.Interfaces; 

var builder = WebApplication.CreateBuilder(args);


// Veritabanı bağlantısını Dependency Injection sistemine ekler.
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString =
        builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException(
            "DefaultConnection bağlantı bilgisi bulunamadı.");

    options.UseSqlServer(connectionString);
});
builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection("Jwt"));
    var jwtOptions = builder.Configuration
    .GetSection("Jwt")
    .Get<JwtOptions>()
    ?? throw new InvalidOperationException(
        "JWT ayarları bulunamadı."
    );

builder.Services
    .AddAuthentication(
        JwtBearerDefaults.AuthenticationScheme
    )
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
                        Encoding.UTF8.GetBytes(
                            jwtOptions.Key
                        )
                    ),

                ClockSkew = TimeSpan.Zero
            };
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();