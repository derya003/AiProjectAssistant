using AiProjectAssistant.Api.DTOs.Auth;
using AiProjectAssistant.Api.Repositories.Interfaces;
using AiProjectAssistant.Api.Services.Interfaces;

namespace AiProjectAssistant.Api.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public AuthService(
        IUserRepository userRepository,
        IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<LoginResponseDto?> LoginAsync(
        LoginRequestDto request)
    {
        var user = await _userRepository
            .GetByEmailAsync(request.Email);

        if (user is null)
        {
            return null;
        }

        var passwordIsValid = BCrypt.Net.BCrypt.Verify(
            request.Password,
            user.PasswordHash
        );

        if (!passwordIsValid)
        {
            return null;
        }

        var token = _jwtService.GenerateToken(user);

        return new LoginResponseDto
        {
            Token = token
        };
    }
}