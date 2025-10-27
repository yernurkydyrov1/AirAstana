using AirAstana.Application.Common;
using AirAstana.Domain.Entities;
using MediatR;

namespace AirAstana.Application.Commands;

public record UpdateFlightStatusCommand(
    int FlightId,
    FlightStatus Status) : IRequest<OperationResult<object>>;