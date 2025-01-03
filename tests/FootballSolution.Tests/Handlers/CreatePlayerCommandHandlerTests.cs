namespace FootballSolution.Tests.Handlers;

using Application.Common.Interfaces;
using Application.Features.Command.CreatePlayer;
using FluentAssertions;
using global::Domain.Entities;
using global::Domain.Enum;
using Moq;

public class CreatePlayerCommandHandlerTests
{
    private readonly Mock<IPlayerRepository> _playerRepository;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly CreatePlayerCommandHandler _handler;

    public CreatePlayerCommandHandlerTests()
    {
        _playerRepository = new Mock<IPlayerRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _handler = new CreatePlayerCommandHandler(_playerRepository.Object, _unitOfWork.Object);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldCreatePlayer()
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

        Player? capturedPlayer = null;
        _playerRepository.Setup(r => r.AddAsync(It.IsAny<Player>(), default))
            .Callback<Player, CancellationToken>((p, _) => capturedPlayer = p);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBe(Guid.Empty);
        
        _playerRepository.Verify(r => r.AddAsync(It.IsAny<Player>(), default), Times.Once);
        _unitOfWork.Verify(u => u.SaveChangesAsync(default), Times.Once);

        capturedPlayer.Should().NotBeNull();
        capturedPlayer!.GetFullName().Should().Be("John Doe");
        capturedPlayer.GetPosition().Should().Be(Position.Forward);
    }

    [Fact]
    public async Task Handle_WhenRepositoryFails_ShouldReturnFailure()
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

        _playerRepository.Setup(r => r.AddAsync(It.IsAny<Player>(), default))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, default));
        _unitOfWork.Verify(u => u.SaveChangesAsync(default), Times.Never);
    }
}