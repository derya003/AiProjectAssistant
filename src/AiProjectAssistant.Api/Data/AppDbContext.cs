using AiProjectAssistant.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace AiProjectAssistant.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;

    public DbSet<Project> Projects { get; set; } = null!;
}