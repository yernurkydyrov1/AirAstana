using AirAstana.Domain.Entities;
using AirAstana.Infrastructure.Data;
using AirAstana.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AirAstana.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;
    public UserRepository(AppDbContext db) => _db = db;

    public Task<User?> GetByUsernameAsync(string username) =>
        _db.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Username == username);

    public async Task AddAsync(User user)
    {
        await _db.Users.AddAsync(user);
        await _db.SaveChangesAsync();
    }
}