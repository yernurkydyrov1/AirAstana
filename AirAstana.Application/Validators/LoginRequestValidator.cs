using AirAstana.Application.Common;
using FluentValidation;
using AirAstana.Application.DTOs;

namespace AirAstana.Application.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage(Messages.UsernameRequired);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(Messages.PasswordRequired);
    }
}