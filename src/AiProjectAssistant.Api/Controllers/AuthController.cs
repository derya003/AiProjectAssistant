using AiProjectAssistant.Api.DTOs.Auth;
using AiProjectAssistant.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AiProjectAssistant.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDto request)
    {
        var response = await _authService.LoginAsync(request);

        if (response is null)
        {
            return Unauthorized(new
            {
                message = "E-posta veya şifre hatalı."
            });
        }

        return Ok(response);
    }
}