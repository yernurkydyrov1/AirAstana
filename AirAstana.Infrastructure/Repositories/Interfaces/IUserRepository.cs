using AirAstana.Domain.Entities;

namespace AirAstana.Infrastructure.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);
    Task AddAsync(User user);
}