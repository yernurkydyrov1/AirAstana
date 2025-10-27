using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AirAstana.Application.Commands;
using AirAstana.Application.Queries;
using AirAstana.Application.Common;
using AirAstana.Application.DTOs;
using AirAstana.Domain.Entities;
using AirAstana.Presentation.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace AirAstana.Tests;

public class FlightsControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly FlightsController _controller;

    public FlightsControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new FlightsController(_mediatorMock.Object);
    }

    [Fact]
    public async Task Get_ShouldReturnOk_WithFlightsList()
    {
        // Arrange
        var flightsDto = new List<FlightDto>
        {
            new(
                Id: 1, 
                Origin: "ALA", 
                Destination: "AST", 
                Departure: DateTimeOffset.Now.AddHours(1),
                Arrival: DateTimeOffset.Now.AddHours(3),
                Status: FlightStatus.Cancelled)
        };
        var expectedResult = new OperationResult<List<FlightDto>>(true, Messages.FlightsFound, flightsDto);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetFlightsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.Get(null, null, null, null, null);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var operationResult = Assert.IsType<OperationResult<List<FlightDto>>>(okResult.Value);
        Assert.True(operationResult.Success);
        if (operationResult.Data != null)
        {
            Assert.Single(operationResult.Data);
            Assert.Equal("ALA", operationResult.Data[0].Origin);
        }
    }
    [Fact]
    public async Task Create_ShouldReturnOk_WhenFlightCreated()
    {
        // Arrange
        var command = new AddFlightCommand("ALA", "AST", DateTimeOffset.Now, DateTimeOffset.Now.AddHours(2), FlightStatus.Cancelled);
        var expectedResult = new OperationResult<int>(true, Messages.FlightAdded, 1);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<AddFlightCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.Create(command);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var operationResult = Assert.IsType<OperationResult<int>>(okResult.Value);
        Assert.True(operationResult.Success);
        Assert.Equal(1, operationResult.Data);
        Assert.Equal(Messages.FlightAdded, operationResult.Message);
    }

    [Fact]
    public async Task UpdateStatus_ShouldReturnOk_WhenStatusUpdated()
    {
        // Arrange
        const int flightId = 1;
        var command = new UpdateFlightStatusCommand(flightId, FlightStatus.Cancelled);
        var expectedResult = new OperationResult<object>(true, Messages.FlightStatusUpdated);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<UpdateFlightStatusCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.UpdateStatus(flightId, command);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var operationResult = Assert.IsType<OperationResult<object>>(okResult.Value);
        Assert.True(operationResult.Success);
        Assert.Equal(Messages.FlightStatusUpdated, operationResult.Message);
    }
}