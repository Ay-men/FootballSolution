namespace FootballSolution.Tests.Controllers;

using API.Controllers;
using Application.Common.Models;
using Application.Features.Command.CreatePlayer;
using Application.Features.Query.GetPlayerById;
using FluentAssertions;
using global::Domain.Common;
using global::Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

public class PlayersControllerTests
{
    private readonly Mock<IMediator> _mediator;
    private readonly PlayersController _controller;

    public PlayersControllerTests()
    {
        _mediator = new Mock<IMediator>();
        _controller = new PlayersController(_mediator.Object);
    }

    [Fact]
    public async Task Create_WithValidCommand_ReturnsCreatedResponse()
    {
        // Arrange
        var command = new CreatePlayerCommand(
            "John",
            "Doe",
            new DateOnly(2000, 1, 1),
            1.80m,
            Position.Forward,
            1000000m,
            "USD",
            "john@example.com",
            "+1234567890",
            "USA",
            "AB123456");

        var playerId = Guid.NewGuid();
        _mediator.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<Guid>.Success(playerId));

        // Act
        var result = await _controller.Create(command);

        // Assert
        var createdResult = result.Result as CreatedAtActionResult;
        createdResult.Should().NotBeNull();
        createdResult!.ActionName.Should().Be(nameof(PlayersController.GetById));
        createdResult.RouteValues!["id"].Should().Be(playerId);
    }

    [Fact]
    public async Task Create_WhenValidationFails_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreatePlayerCommand(
            "",  // Invalid name
            "Doe",
            new DateOnly(2000, 1, 1),
            1.80m,
            Position.Forward,
            1000000m,
            "USD",
            "john@example.com",
            "+1234567890",
            "USA",
            "AB123456");

        _mediator.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<Guid>.Failure(Error.Validation("Invalid name")));

        // Act
        var result = await _controller.Create(command);

        // Assert
        var badRequestResult = result.Result as BadRequestObjectResult;
        badRequestResult.Should().NotBeNull();
    }

    [Fact]
    public async Task GetById_WithExistingPlayer_ReturnsOkResponse()
    {
        // Arrange
        var playerId = Guid.NewGuid();
        var playerResponse = new PlayerResponse(
            playerId,
            "John",
            "Doe",
            new DateOnly(2000, 1, 1),
            1.80m,
            Position.Forward,
            1000000m);

        _mediator.Setup(m => m.Send(
            It.Is<GetPlayerByIdQuery>(q => q.PlayerId == playerId),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<PlayerResponse>.Success(playerResponse));

        // Act
        var result = await _controller.GetById(playerId);

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        var value = okResult!.Value as Result<PlayerResponse>;
        value!.Value.Should().BeEquivalentTo(playerResponse);
    }
}