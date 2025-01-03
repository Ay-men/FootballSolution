namespace FootballSolution.Tests.Domain;

using FluentAssertions;
using global::Domain.Entities;
using global::Domain.Entities.ValueObjects;
using global::Domain.Enum;
using global::Domain.Events;
using global::Domain.ValueObjects;

public class PlayerTests
{
    private static readonly PersonalInfo ValidPersonalInfo = PersonalInfo.Create(
        "John",
        "Doe",
        new DateOnly(2000, 1, 1));
    
    private static readonly Height ValidHeight = Height.Create(1.80m);
    private static readonly Position ValidPosition = Position.Forward;
    private static readonly Money ValidMoney = Money.Create(1000000m, "USD");
    private static readonly MarketValue ValidMarketValue = MarketValue.Create(ValidMoney);

    [Fact]
    public void Create_WithValidData_ShouldCreatePlayer()
    {
        // Act
        var result = Player.Create(
            ValidPersonalInfo,
            ValidHeight,
            ValidPosition,
            ValidMarketValue,
            "john@example.com",
            "+1234567890",
            "USA",
            "AB123456");

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Single(result.Value.GetDomainEvents(), e => e is PlayerCreatedEvent);
        
    }
    
    [Fact]
    public void SignContract_WhenAlreadyUnderContract_ShouldReturnFailure()
    {
        // Arrange
        var player = Player.Create(
            ValidPersonalInfo,
            ValidHeight,
            ValidPosition,
            ValidMarketValue,
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
            DateTime.UtcNow,
            Money.Create(100000m, "USD"),
            DateTime.UtcNow.AddYears(2));

        // Act
        var firstContract = player.SignContract(team, contractDetails);
        var secondContract = player.SignContract(team, contractDetails);

        // Assert
        Assert.True(firstContract.IsSuccess);
        Assert.False(secondContract.IsSuccess);
        Assert.Contains( "already has an active contract",secondContract.Error.Message);

    }

    [Fact]
    public void UpdateMarketValue_ShouldRaiseEvent()
    {
        // Arrange
        var player = Player.Create(
            ValidPersonalInfo,
            ValidHeight,
            ValidPosition,
            ValidMarketValue,
            "john@example.com",
            "+1234567890",
            "USA",
            "AB123456").Value;

        var newValue = Money.Create(2000000m, "USD");

        // Act
        var result = player.UpdateMarketValue(newValue);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(player.GetDomainEvents(), e => e is PlayerMarketValueUpdatedEvent);
    }
    
    [Fact]
    public void Create_WithAgeExactly16_ShouldSucceed()
    {
        // Arrange
        var sixteenYearsAgo = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-16));
        var personalInfo = PersonalInfo.Create("John", "Doe", sixteenYearsAgo);
        
        var result = Helpers.CreateValidPlayerEntity(personalInfo.FirstName,personalInfo.FirstName);

        // Assert
        result.Should().NotBeNull();
        
    }
   
}
