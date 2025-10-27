using AirAstana.Application.Common;
using AirAstana.Domain.Entities;
using AirAstana.Infrastructure.Repositories;
using AirAstana.Infrastructure.Repositories.Interfaces;
using MediatR;

namespace AirAstana.Application.Commands.Handler;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, OperationResult<object>>
{
    private readonly IUserRepository _userRepository;

    private const int DefaultUserRoleId = 1;

    public RegisterUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<OperationResult<object>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.GetByUsernameAsync(request.Username);
        if (existingUser != null)
        {
            return new OperationResult<object>(
                Success: false,
                Message: Messages.UserAlreadyExists,
                Code: HttpCode.ValidationError
            );
        }

        var roleId = request.RoleId ?? DefaultUserRoleId;

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = new User
        {
            Username = request.Username,
            PasswordHash = hashedPassword,
            RoleId = roleId
        };

        await _userRepository.AddAsync(user);

        return new OperationResult<object>(
            Success: true,
            Message: string.Format(Messages.UserRegistered, roleId),
            Code: HttpCode.Success
        );
    }
}