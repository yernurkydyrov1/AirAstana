using AirAstana.Application.Common;
using AirAstana.Application.DTOs;
using AirAstana.Application.Interfaces;
using MediatR;

namespace AirAstana.Application.Queries.Handlers;

public class GetFlightsQueryHandler : IRequestHandler<GetFlightsQuery, OperationResult<List<FlightDto>>>
{
    private readonly IFlightService _service;

    public GetFlightsQueryHandler(IFlightService service)
    {
        _service = service;
    }

    public async Task<OperationResult<List<FlightDto>>> Handle(GetFlightsQuery request, CancellationToken cancellationToken)
    {
        var flights = await _service.GetAllFlightsAsync(
            origin: request.Origin,
            destination: request.Destination,
            departureFrom: request.DepartureFrom,
            departureTo: request.DepartureTo,
            status: request.Status
        );

        var dtos = flights.Select(f => new FlightDto(
            f.Id,
            f.Origin,
            f.Destination,
            f.Departure,
            f.Arrival,
            f.Status
        )).ToList();


        return new OperationResult<List<FlightDto>>(
            Success: true,
            Message: flights.Any() ? Messages.FlightsFound : Messages.FlightsNotFound,
            Data: dtos,
            Code: HttpCode.Success
        );
    }

}