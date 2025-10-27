using AirAstana.Domain.Entities;

namespace AirAstana.Application.DTOs;

public record FlightDto(
    int Id,
    string Origin,
    string Destination,
    DateTimeOffset Departure,
    DateTimeOffset Arrival,
    FlightStatus Status
);