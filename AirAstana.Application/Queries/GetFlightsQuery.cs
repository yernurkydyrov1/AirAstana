using AirAstana.Application.Common;
using MediatR;
using AirAstana.Application.DTOs;

namespace AirAstana.Application.Queries;

public record GetFlightsQuery(
    string? Origin,
    string? Destination,
    DateTime? DepartureFrom,
    DateTime? DepartureTo,
    int? Status
) : IRequest<OperationResult<List<FlightDto>>>;