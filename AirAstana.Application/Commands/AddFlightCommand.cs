using AirAstana.Application.Common;
using AirAstana.Domain.Entities;
using MediatR;

namespace AirAstana.Application.Commands
{
    public record AddFlightCommand(
        string Origin,
        string Destination,
        DateTimeOffset Departure,
        DateTimeOffset Arrival,
        FlightStatus Status
    ) : IRequest<OperationResult<int>>;

}
