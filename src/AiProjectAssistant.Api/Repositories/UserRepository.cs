using AiProjectAssistant.Api.Data;
using AiProjectAssistant.Api.Entities;
using AiProjectAssistant.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AiProjectAssistant.Api.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(user => user.Email == email);
    }
}