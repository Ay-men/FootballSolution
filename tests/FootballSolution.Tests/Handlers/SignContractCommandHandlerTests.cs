namespace FootballSolution.Tests.Handlers;

using Application.Common.Interfaces;
using Application.Features.Command.SignContract;
using FluentAssertions;
using global::Domain.Entities;
using global::Domain.Entities.ValueObjects;
using global::Domain.Enum;
using global::Domain.ValueObjects;
using Moq;

public class SignContractCommandHandlerTests
{
     private readonly Mock<IPlayerRepository> _playerRepository;
    private readonly Mock<ITeamRepository> _teamRepository;
    private readonly Mock<IContractRepository> _contractRepository;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly SignContractCommandHandler _handler;

    public SignContractCommandHandlerTests()
    {
        _playerRepository = new Mock<IPlayerRepository>();
        _teamRepository = new Mock<ITeamRepository>();
        _contractRepository = new Mock<IContractRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _handler = new SignContractCommandHandler(
            _playerRepository.Object,
            _teamRepository.Object,
            _contractRepository.Object,
            _unitOfWork.Object);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldCreateContract()
    {
        // Arrange
        var playerId = Guid.NewGuid();
        var teamId = Guid.NewGuid();
        var command = new SignContractCommand(
            playerId,
            teamId,
            DateTime.UtcNow.AddDays(1),
            DateTime.UtcNow.AddYears(2),
            100000m,
            "USD");

        var player = Player.Create(
            PersonalInfo.Create("John", "Doe", new DateOnly(2000, 1, 1)),
            Height.Create(1.80m),
            Position.Forward,
            MarketValue.Create(Money.Create(1000000m, "USD")),
            "john@example.com",
            "+1234567890",
            "USA",
            "AB123456").Value;

        var team = Team.Create(
            "Test FC",
            "Test City",
            "Test Country",
            1900,
            "Test Stadium",
            Money.Create(10000000m, "USD"));

        _playerRepository.Setup(r => r.GetPlayerByIdAsync(It.IsAny<PlayerId>(), default))
            .ReturnsAsync(player);
        _teamRepository.Setup(r => r.GetByIdAsync(It.IsAny<TeamId>(), default))
            .ReturnsAsync(team);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }
}