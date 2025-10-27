using AirAstana.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AirAstana.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<Flight> Flights => Set<Flight>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>().HasIndex(u => u.Username).IsUnique();
        builder.Entity<Role>().HasIndex(r => r.Code).IsUnique();
        base.OnModelCreating(builder);
    }
}