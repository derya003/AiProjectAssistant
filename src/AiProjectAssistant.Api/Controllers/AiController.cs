using AiProjectAssistant.Api.DTOs.AI;
using AiProjectAssistant.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AiProjectAssistant.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AiController : ControllerBase
{
    private readonly IAiService _aiService;

    public AiController(IAiService aiService)
    {
        _aiService = aiService;
    }

    [HttpPost("ask")]
    public async Task<ActionResult<AskResponseDto>> Ask(
        AskRequestDto request)
    {
        if (request.ProjectId <= 0)
        {
            return BadRequest(
                "Geçerli bir proje kimliği girilmelidir.");
        }

        if (string.IsNullOrWhiteSpace(request.Question))
        {
            return BadRequest(
                "Soru alanı boş bırakılamaz.");
        }

        var response = await _aiService.AskAsync(request);

        if (response is null)
        {
            return NotFound(
                "Belirtilen proje bulunamadı.");
        }

        return Ok(response);
    }
}