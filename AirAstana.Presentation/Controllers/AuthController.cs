using AirAstana.Application.Commands;
using AirAstana.Application.Common;
using AirAstana.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AirAstana.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Регистрация нового пользователя.
    /// </summary>
    /// <param name="request">Имя пользователя, пароль и роль.</param>
    /// <remarks>
    /// Роли:
    /// - 1 — User
    /// - 2 — Moderator
    /// </remarks>
    /// <returns>Результат операции с сообщением о регистрации.</returns>
    [HttpPost("register")]
    [SwaggerOperation(Summary = "Регистрация пользователя", Description = "Создает нового пользователя с заданной ролью")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(OperationResult<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody, SwaggerRequestBody(Required = true, Description = "Пример запроса")]
        RegisterRequest request)
    {
        var result = await _mediator.Send(new RegisterUserCommand(request.Username, request.Password, request.RoleId));
        return Ok(result);
    }

    /// <summary>
    /// Вход пользователя.
    /// </summary>
    /// <param name="request">Имя пользователя и пароль.</param>
    /// <returns>JWT токен при успешном входе.</returns>
    [HttpPost("login")]
    [SwaggerOperation(Summary = "Вход пользователя", Description = "Проверяет имя пользователя и пароль и возвращает JWT токен")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(OperationResult<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody, SwaggerRequestBody(Required = true, Description = "Пример запроса")]
        LoginRequest request)
    {
        var result = await _mediator.Send(new LoginUserCommand(request.Username, request.Password));
        return Ok(result);
    }
}