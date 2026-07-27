using AiProjectAssistant.Api.Data;
using AiProjectAssistant.Api.Entities;
using AiProjectAssistant.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AiProjectAssistant.Api.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly AppDbContext _context;

    public ProjectRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Project?> GetByIdAsync(int projectId)
    {
        return await _context.Projects
            .FirstOrDefaultAsync(project => project.Id == projectId);
    }
}