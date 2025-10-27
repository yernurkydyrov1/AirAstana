using AirAstana.Application.Common;
using MediatR;

namespace AirAstana.Application.Commands;

public record LoginUserCommand(string Username, string Password) 
    : IRequest<OperationResult<string>>;
