using AiProjectAssistant.Api.Entities;

namespace AiProjectAssistant.Api.Repositories.Interfaces;

public interface IProjectRepository
{
    Task<Project?> GetByIdAsync(int projectId);
}