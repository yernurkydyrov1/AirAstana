using AirAstana.Application.Commands;
using AirAstana.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AirAstana.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FlightsController : ControllerBase
{
    private readonly IMediator _mediator;

    public FlightsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Получение рейсов с фильтрацией.
    /// </summary>
    /// <param name="origin">Город отправления (необязательный).</param>
    /// <param name="destination">Город назначения (необязательный).</param>
    /// <param name="departureFrom">Дата вылета с (необязательный).</param>
    /// <param name="departureTo">Дата вылета до (необязательный).</param>
    /// <param name="status">Статус рейса (необязательный).</param>
    /// <remarks>
    /// Доступно для всех авторизованных пользователей (User, Moderator).
    /// </remarks>
    /// <returns>Список рейсов с результатом операции.</returns>
    [HttpGet]
    [Authorize]
    [SwaggerOperation(Summary = "Получение рейсов", Description = "Позволяет фильтровать рейсы по параметрам")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Get(
        [FromQuery] string? origin,
        [FromQuery] string? destination,
        [FromQuery] DateTime? departureFrom,
        [FromQuery] DateTime? departureTo,
        [FromQuery] int? status)
    {
        var query = new GetFlightsQuery(origin, destination, departureFrom, departureTo, status);
        var result = await _mediator.Send(query);

        return Ok(result); 
    }

    /// <summary>
    /// Создание нового рейса.
    /// </summary>
    /// <remarks>
    /// Доступно только модераторам:
    /// - Moderator: роль 2
    /// </remarks>
    [HttpPost]
    [Authorize(Policy = "ModeratorPolicy")]
    [SwaggerOperation(Summary = "Создание рейса", Description = "Создает новый рейс (только для модераторов)")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Create([FromBody] AddFlightCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Обновление статуса рейса.
    /// </summary>
    /// <param name="id">ID рейса.</param>
    /// <param name="command">Новый статус рейса.</param>
    /// <remarks>
    /// Доступно только модераторам:
    /// - Moderator: роль 2
    /// </remarks>
    [HttpPatch("{id:int}/status")]
    [Authorize(Policy = "ModeratorPolicy")]
    [SwaggerOperation(Summary = "Обновление статуса рейса", Description = "Обновляет статус рейса по ID (только для модераторов)")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateFlightStatusCommand command)
    {
        var updatedCommand = command with { FlightId = id };
        var result = await _mediator.Send(updatedCommand);

        return Ok(result); 
    }
}
