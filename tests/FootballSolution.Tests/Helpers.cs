namespace FootballSolution.Tests;

using global::Domain.Common;
using global::Domain.Entities;
using global::Domain.Entities.ValueObjects;
using global::Domain.Enum;
using global::Domain.ValueObjects;
using Infrastructure.Persistence.Models;

public static class Helpers
{
    public static PlayerEntity CreateValidPlayerEntity(
        string firstName = "John",
        string lastName = "Doe",
        Guid? teamId = null,
        Position position = Position.Forward)
    {
        return new PlayerEntity
        {
            Id = Guid.NewGuid(),
            FirstName = firstName,
            LastName = lastName,
            DateOfBirth = new DateTime(2000, 1, 1),
            Height = 1.80m,
            Position = (int)position,
            MarketValue = 1000000m,
            Email = $"{firstName.ToLower()}@example.com",
            Phone = "+1234567890",
            Nationality = "USA",
            PassportNumber = "AB123456",
            CreatedAt = DateTime.UtcNow
        };
    }
    public static Result<Team> CreateValidTeam()
    {
        return Result<Team>.Success(Team.Create(
            "Test FC",
            "Test City",
            "Test Country",
            1900,
            "Test Stadium",
            Money.Create(10000000m, "USD")));
    }
    public static Result<Player> CreateValidPlayer(PersonalInfo? personalInfo = null)
    {
        return Player.Create(
            personalInfo ?? PersonalInfo.Create("John", "Doe", new DateOnly(2000, 1, 1)),
            Height.Create(1.80m),
            Position.Forward,
            MarketValue.Create(Money.Create(1000000m, "USD")),
            "john@example.com",
            "+1234567890",
            "USA",
            "AB123456");
    }
}