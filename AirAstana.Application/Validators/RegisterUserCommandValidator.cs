using FluentValidation;
using AirAstana.Application.Commands;
using AirAstana.Application.Common;

namespace AirAstana.Application.Validators;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage(Messages.UsernameRequired)
            .MinimumLength(3).WithMessage(Messages.UsernameMinLength);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(Messages.PasswordRequired)
            .MinimumLength(6).WithMessage(Messages.PasswordMinLength);
    }
}