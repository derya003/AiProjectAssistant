using AiProjectAssistant.Api.DTOs.AI;

namespace AiProjectAssistant.Api.Services.Interfaces;

public interface IAiService
{
    Task<AskResponseDto?> AskAsync(AskRequestDto request);
}