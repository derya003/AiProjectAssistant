using AiProjectAssistant.Api.Entities;

namespace AiProjectAssistant.Api.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
}