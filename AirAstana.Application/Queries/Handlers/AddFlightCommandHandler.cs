using AirAstana.Application.Commands;
using AirAstana.Application.Common;
using AirAstana.Application.Interfaces;
using AirAstana.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AirAstana.Application.Queries.Handlers;

public class AddFlightCommandHandler : IRequestHandler<AddFlightCommand, OperationResult<int>>
{
    private readonly IFlightService _service;
    private readonly ILogger<AddFlightCommandHandler> _logger;

    public AddFlightCommandHandler(IFlightService service, ILogger<AddFlightCommandHandler> logger)
    {
        _service = service;
        _logger = logger;
    }

    public async Task<OperationResult<int>> Handle(AddFlightCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.Departure >= request.Arrival)
            {
                return new OperationResult<int>(
                    Success: false,
                    Message: Messages.FlightTimeInvalid,
                    Data: 0,
                    Code: HttpCode.ValidationError
                );
            }

            var flight = new Flight
            {
                Origin = request.Origin,
                Destination = request.Destination,
                Departure = request.Departure,
                Arrival = request.Arrival,
                Status = request.Status
            };

            var id = await _service.AddFlightAsync(flight);

            return new OperationResult<int>(
                Success: true,
                Message: Messages.FlightAdded,
                Data: id,
                Code: HttpCode.Success
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, Messages.FlightAddException);

            return new OperationResult<int>(
                Success: false,
                Message: Messages.FlightAddError,
                Data: 0,
                Code: HttpCode.UnknownError
            );
        }
    }
}