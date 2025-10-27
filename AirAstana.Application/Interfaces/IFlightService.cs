using AirAstana.Domain.Entities;

namespace AirAstana.Application.Interfaces;

public interface IFlightService
{
    Task<List<Flight>> GetAllFlightsAsync(
        string? origin, 
        string? destination,
        DateTime? departureFrom,
        DateTime? departureTo,
        int? status
    );
    Task<int> AddFlightAsync(Flight flight);
    Task<bool> UpdateFlightStatusAsync(int id, FlightStatus status);
}