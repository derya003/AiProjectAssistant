using AiProjectAssistant.Api.Entities;

namespace AiProjectAssistant.Api.Services.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user);
}