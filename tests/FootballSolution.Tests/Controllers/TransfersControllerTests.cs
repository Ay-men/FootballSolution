namespace FootballSolution.Tests.Controllers;

using API.Controllers;
using Application.Features.Command.SignContract;
using FluentAssertions;
using global::Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

public class TransfersControllerTests
{
    private readonly Mock<IMediator> _mediator;
    private readonly TransfersController _controller;

    public TransfersControllerTests()
    {
        _mediator = new Mock<IMediator>();
        _controller = new TransfersController(_mediator.Object);
    }

    [Fact]
    public async Task TransferPlayer_WithValidCommand_ReturnsOkResult()
    {
        // Arrange
        var command = new SignContractCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            DateTime.UtcNow.AddDays(1),
            DateTime.UtcNow.AddYears(2),
            100000m,
            "USD");

        var contractId = Guid.NewGuid();
        _mediator.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<Guid>.Success(contractId));

        // Act
        var result = await _controller.TransferPlayer(command);

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        var value = okResult!.Value as Result<Guid>;
        value!.Value.Should().Be(contractId);
    }

    [Fact]
    public async Task TransferPlayer_WithNotFoundError_ReturnsNotFound()
    {
        // Arrange
        var command = new SignContractCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            DateTime.UtcNow.AddDays(1),
            DateTime.UtcNow.AddYears(2),
            100000m,
            "USD");

        _mediator.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<Guid>.Failure(
                Error.NotFound("Player not found")));

        // Act
        var result = await _controller.TransferPlayer(command);

        // Assert
        var notFoundResult = result.Result as NotFoundObjectResult;
        notFoundResult.Should().NotBeNull();
        notFoundResult!.Value.Should().Be("Player not found");
    }
}