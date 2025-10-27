using FluentValidation;
using AirAstana.Application.Commands;

namespace AirAstana.Application.Validators;

public class UpdateFlightStatusCommandValidator : AbstractValidator<UpdateFlightStatusCommand>
{
    public UpdateFlightStatusCommandValidator()
    {
        RuleFor(x => x.FlightId).GreaterThan(0);
        RuleFor(x => x.Status).IsInEnum();
    }
}