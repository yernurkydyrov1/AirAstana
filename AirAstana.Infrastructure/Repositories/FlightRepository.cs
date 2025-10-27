using AirAstana.Domain.Entities;
using AirAstana.Infrastructure.Data;
using AirAstana.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AirAstana.Infrastructure.Repositories;

public class FlightRepository : IFlightRepository
{
    private readonly AppDbContext _db;
    public FlightRepository(AppDbContext db) => _db = db;

    public Task<List<Flight>> GetAllAsync() => _db.Flights.OrderBy(f => f.Arrival).ToListAsync();

    public Task<List<Flight>> GetFilteredAsync(string? origin, string? destination)
    {
        var q = _db.Flights.AsQueryable();
        if (!string.IsNullOrEmpty(origin)) q = q.Where(f => f.Origin == origin);
        if (!string.IsNullOrEmpty(destination)) q = q.Where(f => f.Destination == destination);
        return q.OrderBy(f => f.Arrival).ToListAsync();
    }

    public Task<Flight?> GetByIdAsync(int id) => _db.Flights.FindAsync(id).AsTask();

    public async Task AddAsync(Flight flight)
    {
        await _db.Flights.AddAsync(flight);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Flight flight)
    {
        _db.Flights.Update(flight);
        await _db.SaveChangesAsync();
    }
        
    public IQueryable<Flight> Query()
    {
        return _db.Flights.AsQueryable();
    }

}