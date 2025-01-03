namespace FootballSolution.Tests.Domain;

using global::Domain.Entities;
using global::Domain.Entities.ValueObjects;
using global::Domain.Enum;
using global::Domain.ValueObjects;

public class ContractTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateContract()
    {
        // Arrange
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

        var contractDetails = new ContractDetails(
            DateTime.UtcNow.AddDays(1),
            Money.Create(100000m, "USD"),
            DateTime.UtcNow.AddYears(2));

        // Act
        var contract = Contract.Create(player, team, contractDetails);

        // Assert
        
        
        Assert.NotNull(contract);
        Assert.Equal(contract.GetPlayerId(), player.Id);
        Assert.Equal(contract.GetTeamId(), team.Id);
        Assert.Equal(contract.GetSalary(), 100000m);

    }

    [Fact]
    public void IsActive_ShouldReturnCorrectStatus()
    {
        // Arrange
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

        // Test future contract
        var futureContract = Contract.Create(player, team, new ContractDetails(
            DateTime.UtcNow.AddDays(1),
            Money.Create(100000m, "USD"),
            DateTime.UtcNow.AddYears(2)));

        // Test active contract
        var activeContract = Contract.Create(player, team, new ContractDetails(
            DateTime.UtcNow.AddDays(-1),
            Money.Create(100000m, "USD"),
            DateTime.UtcNow.AddYears(2)));

        // Test expired contract
        var expiredContract = Contract.Create(player, team, new ContractDetails(
            DateTime.UtcNow.AddYears(-2),
            Money.Create(100000m, "USD"),
            DateTime.UtcNow.AddDays(-1)));

        // Assert
       Assert.False(futureContract.IsActive());
       Assert.True(activeContract.IsActive());
       Assert.False(expiredContract.IsActive());
       // futureContract.IsActive().Should().BeFalse();
       // activeContract.IsActive().Should().BeTrue();
       //  expiredContract.IsActive().Should().BeFalse();
    }
}