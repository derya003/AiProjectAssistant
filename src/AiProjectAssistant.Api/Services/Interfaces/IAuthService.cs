using AiProjectAssistant.Api.DTOs.Auth;

namespace AiProjectAssistant.Api.Services.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);
}