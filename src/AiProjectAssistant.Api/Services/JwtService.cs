using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AiProjectAssistant.Api.Entities;
using AiProjectAssistant.Api.Options;
using AiProjectAssistant.Api.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AiProjectAssistant.Api.Services;

public class JwtService : IJwtService
{
    private readonly JwtOptions _jwtOptions;

    public JwtService(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }

    public string GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(
                JwtRegisteredClaimNames.Sub,
                user.Id.ToString()
            ),

            new Claim(
                JwtRegisteredClaimNames.Email,
                user.Email
            ),

            new Claim(
                JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString()
            )
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtOptions.Key)
        );

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256
        );

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                _jwtOptions.ExpirationMinutes
            ),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}