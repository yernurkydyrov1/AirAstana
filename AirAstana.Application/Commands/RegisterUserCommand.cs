using AirAstana.Application.Common;
using MediatR;

namespace AirAstana.Application.Commands;

public record RegisterUserCommand(string Username, string Password, int? RoleId = null)
    : IRequest<OperationResult<object>>;