using AirAstana.Application.Commands;
using AirAstana.Application.Common;
using AirAstana.Application.Interfaces;
using MediatR;

namespace AirAstana.Application.Queries.Handlers;

public class UpdateFlightStatusCommandHandler : IRequestHandler<UpdateFlightStatusCommand, OperationResult<object>>
{
    private readonly IFlightService _service;

    public UpdateFlightStatusCommandHandler(IFlightService service)
    {
        _service = service;
    }

    public async Task<OperationResult<object>> Handle(UpdateFlightStatusCommand request, CancellationToken cancellationToken)
    {
        var updated = await _service.UpdateFlightStatusAsync(request.FlightId, request.Status);

        if (!updated)
        {
            return new OperationResult<object>(
                Success: false,
                Message: Messages.FlightNotFound,
                Code: HttpCode.NotFound
            );
        }

        return new OperationResult<object>(
            Success: true,
            Message: Messages.FlightStatusUpdated,
            Code: HttpCode.Success
        );
    }
}