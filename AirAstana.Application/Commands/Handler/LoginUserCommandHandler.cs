using AirAstana.Application.Common;
using AirAstana.Infrastructure.Repositories;
using AirAstana.Infrastructure.Repositories.Interfaces;
using AirAstana.Infrastructure.Services;
using MediatR;

namespace AirAstana.Application.Commands.Handler;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, OperationResult<string>>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenService _jwt;

    public LoginUserCommandHandler(IUserRepository userRepository, IJwtTokenService jwt)
    {
        _userRepository = userRepository;
        _jwt = jwt;
    }

    public async Task<OperationResult<string>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
        {
            return new OperationResult<string>(
                Success: false,
                Message: Messages.UsernameOrPasswordRequired,
                Code: HttpCode.ValidationError
            );
        }

        var user = await _userRepository.GetByUsernameAsync(request.Username);
        if (user == null)
        {
            return new OperationResult<string>(
                Success: false,
                Message: Messages.UserNotFound,
                Code: HttpCode.NotFound
            );
        }

        var passwordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
        if (!passwordValid)
        {
            return new OperationResult<string>(
                Success: false,
                Message: Messages.InvalidCredentials,
                Code: HttpCode.Unauthorized
            );
        }

        var token = _jwt.GenerateToken(user);
        return new OperationResult<string>(
            Success: true,
            Message: Messages.LoginSuccess,
            Data: token,
            Code: HttpCode.Success
        );
    }
}