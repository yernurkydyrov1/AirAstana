using AirAstana.Application.Commands;
using AirAstana.Application.Common;
using FluentValidation;

namespace AirAstana.Application.Validators;

public class AddFlightCommandValidator : AbstractValidator<AddFlightCommand>
{
    [Obsolete("Obsolete")]
    public AddFlightCommandValidator()
    {
        // Устанавливаем глобальный режим остановки после первой ошибки на каждом поле
        CascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Origin)
            .NotEmpty().WithMessage(Messages.OriginRequired);

        RuleFor(x => x.Destination)
            .NotEmpty().WithMessage(Messages.DestinationRequired);

        RuleFor(x => x.Departure)
            .LessThan(x => x.Arrival)
            .WithMessage(Messages.FlightTimeInvalid);

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage(Messages.InvalidFlightStatus);
    }
}