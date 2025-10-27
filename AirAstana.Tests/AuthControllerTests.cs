using System.Threading;
using System.Threading.Tasks;
using AirAstana.Application.Commands;
using AirAstana.Application.Common;
using AirAstana.Application.DTOs;
using AirAstana.Presentation.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace AirAstana.Tests;

public class AuthControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new AuthController(_mediatorMock.Object);
    }

    [Fact]
    public async Task Register_ShouldReturnOk_WithSuccessResult()
    {
        // Arrange
        var request = new RegisterRequest("testuser", "password123", 1);
        var expectedResult = new OperationResult<object>(true, Messages.UserRegistered);
            
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<RegisterUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.Register(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var operationResult = Assert.IsType<OperationResult<object>>(okResult.Value);
        Assert.True(operationResult.Success);
        Assert.Equal(Messages.UserRegistered, operationResult.Message);
    }

    [Fact]
    public async Task Login_ShouldReturnOk_WithToken_WhenCredentialsValid()
    {
        // Arrange
        var request = new LoginRequest("testuser", "password123");
        const string expectedToken = "jwt-token";
        var expectedResult = new OperationResult<string>(true, Messages.LoginSuccess, expectedToken);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<LoginUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.Login(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var operationResult = Assert.IsType<OperationResult<string>>(okResult.Value);
        Assert.True(operationResult.Success);
        Assert.Equal(Messages.LoginSuccess, operationResult.Message);
        Assert.Equal(expectedToken, operationResult.Data);
    }

    [Fact]
    public async Task Login_ShouldReturnOk_WithFailure_WhenUserNotFound()
    {
        // Arrange
        var request = new LoginRequest("unknown", "password123");
        var expectedResult = new OperationResult<string>(false, Messages.UserNotFound);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<LoginUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.Login(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var operationResult = Assert.IsType<OperationResult<string>>(okResult.Value);
        Assert.False(operationResult.Success);
        Assert.Equal(Messages.UserNotFound, operationResult.Message);
        Assert.Null(operationResult.Data);
    }
}