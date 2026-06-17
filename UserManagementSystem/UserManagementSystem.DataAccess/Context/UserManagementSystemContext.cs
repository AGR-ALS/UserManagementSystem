using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UserManagementSystem.DataAccess.Entities;

namespace UserManagementSystem.DataAccess.Context;

public class UserManagementSystemContext : DbContext
{
    private readonly IConfiguration _configuration;

    public UserManagementSystemContext(DbContextOptions<UserManagementSystemContext> options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString("PostgreSQLConnectionString"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserManagementSystemContext).Assembly);
    }
    
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }
    public DbSet<AccountConfirmationTokenEntity> AccountConfirmationTokens { get; set; }
}