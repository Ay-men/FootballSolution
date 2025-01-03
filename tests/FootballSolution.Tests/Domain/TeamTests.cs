namespace FootballSolution.Tests.Domain;

using FluentAssertions;
using global::Domain.Entities;
using global::Domain.Entities.ValueObjects;
using global::Domain.Enum;
using global::Domain.Events;
using global::Domain.Exceptions;
using global::Domain.ValueObjects;

public class TeamTests
{
    private static readonly Money ValidBudget = Money.Create(10000000m, "USD");

    [Fact]
    public void Create_WithValidData_ShouldCreateTeam()
    {
        // Act
        var team = Helpers.CreateValidTeam();
        
        // Assert
        Assert.NotNull(team);
        Assert.Single(team.Value.GetDomainEvents(), e => e is TeamCreatedEvent);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    public void Create_WithInvalidName_ShouldThrowDomainException(string invalidName)
    {
        // Act
        Action act = () => Team.Create(
            invalidName,
            "Test City",
            "Test Country",
            1900,
            "Test Stadium",
            ValidBudget);
        
        //Aassert
        DomainException exception = Assert.Throws<DomainException>(act);
        Assert.Equal("Team name cannot be empty", exception.Message);
    
    }

    [Fact]
    public void OfferContract_WhenSalaryExceedsBudget_ShouldThrowDomainException()
    {
        // Arrange
        var team = Team.Create(
            "Test FC",
            "Test City",
            "Test Country",
            1900,
            "Test Stadium",
            Money.Create(100000m, "USD"));

        var player = Player.Create(
            PersonalInfo.Create("John", "Doe", new DateOnly(2000, 1, 1)),
            Height.Create(1.80m),
            Position.Forward,
            MarketValue.Create(Money.Create(1000000m, "USD")),
            "john@example.com",
            "+1234567890",
            "USA",
            "AB123456").Value;

        var contractDetails = new ContractDetails(
            DateTime.UtcNow.AddDays(1),
            Money.Create(200000m, "USD"));

        // Act
        Action act = () => team.OfferContract(player, contractDetails);

        // Assert
        DomainException exception = Assert.Throws<DomainException>(act);
        Assert.Equal("Contract salary exceeds team budget", exception.Message);
    }
    
    [Fact]
    public void UpdateBudget_WithNegativeAmount_ShouldThrowDomainException()
    {
        // Arrange
        var team = Helpers.CreateValidTeam().Value;
        var negativeBudget = Money.Create(-1000m, "USD");

        // Act
        Action act = () => team.UpdateBudget(negativeBudget);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Budget cannot be negative");
    }
    [Fact]
    public void OfferContract_WithZeroSalary_ShouldThrowDomainException()
    {
        // Arrange
        var team = Helpers.CreateValidTeam().Value;
        var player = Helpers.CreateValidPlayer().Value;

        var contractDetails = new ContractDetails(
            DateTime.UtcNow.AddDays(1),
            Money.Create(0m, "USD")); 

        // Act
        Action act = () => team.OfferContract(player, contractDetails);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Salary must be greater than zero");
    }

    
}