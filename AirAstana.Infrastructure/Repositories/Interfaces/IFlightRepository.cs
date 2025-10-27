using AirAstana.Domain.Entities;

namespace AirAstana.Infrastructure.Repositories.Interfaces;

public interface IFlightRepository
{
    Task<List<Flight>> GetAllAsync();
    Task<List<Flight>> GetFilteredAsync(string? origin, string? destination);
    Task<Flight?> GetByIdAsync(int id);
    Task AddAsync(Flight flight);
    Task UpdateAsync(Flight flight);
    IQueryable<Flight> Query(); 

}