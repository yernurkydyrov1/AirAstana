using AirAstana.Application.Interfaces;
using AirAstana.Domain.Entities;
using AirAstana.Infrastructure.Repositories;
using AirAstana.Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;

namespace AirAstana.Application.Services
{
    public class FlightService : IFlightService
    {
        private readonly IFlightRepository _repository;
        private readonly IMemoryCache _memoryCache;
        private const string CacheKey = "flights_cache";

        public FlightService(IFlightRepository repository, IMemoryCache memoryCache)
        {
            _repository = repository;
            _memoryCache = memoryCache;
        }

        public async Task<List<Flight>> GetAllFlightsAsync(
            string? origin, 
            string? destination,
            DateTime? departureFrom,
            DateTime? departureTo,
            int? status)
        {
            var key = CacheKey + $"_{origin}_{destination}_{departureFrom}_{departureTo}_{status}";
            if (_memoryCache.TryGetValue(key, out List<Flight>? cachedFlights)) 
                return cachedFlights!;

            // Используем IQueryable для фильтров и сортировки на уровне БД
            IQueryable<Flight> query = _repository.Query();

            if (!string.IsNullOrEmpty(origin)) query = query.Where(f => f.Origin == origin);
            if (!string.IsNullOrEmpty(destination)) query = query.Where(f => f.Destination == destination);
            if (departureFrom.HasValue) query = query.Where(f => f.Departure >= departureFrom.Value);
            if (departureTo.HasValue) query = query.Where(f => f.Departure <= departureTo.Value);
            if (status.HasValue) query = query.Where(f => (int)f.Status == status.Value);

            // Сортировка по Arrival
            var flights = await query.OrderBy(f => f.Arrival).ToListAsync();

            _memoryCache.Set(key, flights, TimeSpan.FromMinutes(10));
            return flights;
        }

        public async Task<int> AddFlightAsync(Flight flight)
        {
            await _repository.AddAsync(flight);
            await RefreshCacheAsync();
            return flight.Id;
        }

        public async Task<bool> UpdateFlightStatusAsync(int id, FlightStatus status)
        {
            var flight = await _repository.GetByIdAsync(id);
            if (flight == null) return false;

            flight.Status = status;
            await _repository.UpdateAsync(flight);
            await RefreshCacheAsync();
            return true;
        }

        private async Task RefreshCacheAsync()
        {
            var allFlights = await _repository.Query().OrderBy(f => f.Arrival).ToListAsync();
            _memoryCache.Set(CacheKey + "_null_null", allFlights, TimeSpan.FromMinutes(10));
        }
    }
}
