using AiProjectAssistant.Api.DTOs.AI;
using AiProjectAssistant.Api.Repositories.Interfaces;
using AiProjectAssistant.Api.Services.Interfaces;

namespace AiProjectAssistant.Api.Services;

public class AiService : IAiService
{
    private readonly IProjectRepository _projectRepository;
    private readonly IAiProvider _aiProvider;

    public AiService(
        IProjectRepository projectRepository,
        IAiProvider aiProvider)
    {
        _projectRepository = projectRepository;
        _aiProvider = aiProvider;
    }

   public async Task<AskResponseDto?> AskAsync(
    AskRequestDto request)
{
    var project =
        await _projectRepository.GetByIdAsync(
            request.ProjectId);

    if (project is null)
    {
        return null;
    }

    var answer = await _aiProvider.AskAsync(
        project.Prompt,
        request.Question);

    return new AskResponseDto
    {
        Answer = answer
    };
}

}