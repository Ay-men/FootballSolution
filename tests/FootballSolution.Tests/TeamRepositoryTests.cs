namespace FootballSolution.Tests;

using AutoMapper;
using FluentAssertions;
using global::Domain.Entities;
using global::Domain.ValueObjects;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Mapping;
using Infrastructure.Persistence.Models;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

public class TeamRepositoryTests: IClassFixture<DbContextFixture>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly TeamRepository _repository;

    public TeamRepositoryTests(DbContextFixture fixture)
    {
        _context = fixture.Context;
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
        _repository = new TeamRepository(_context, _mapper);
    }

    [Fact]
    public async Task ExistsAsync_WithExistingName_ReturnsTrue()
    {
        // Arrange
        var teamEntity = new TeamEntity
        {
            Id = Guid.NewGuid(),
            Name = "Unique Team Name",
            City = "City",
            Country = "Country",
            FoundedYear = 2000,
            Budget = 1000000m,
            Currency = "USD",
            CreatedAt = DateTime.UtcNow
        };
        await _context.Teams.AddAsync(teamEntity);
        await _context.SaveChangesAsync();

        // Act
        var exists = await _repository.ExistsAsync("Unique Team Name");

        // Assert
        exists.Should().BeTrue();
    }

    [Fact]
    public async Task AddAsync_WithValidTeam_ShouldPersistTeam()
    {
        // Arrange
        var team = Team.Create(
            "New Team",
            "City",
            "Country",
            2000,
            "Stadium",
            Money.Create(1000000m, "USD"));

        // Act
        await _repository.AddAsync(team);
        await _context.SaveChangesAsync();

        // Assert
        var persistedTeam = await _context.Teams.FirstOrDefaultAsync(t => t.Name == "New Team");
        persistedTeam.Should().NotBeNull();
        persistedTeam!.Budget.Should().Be(1000000m);
        persistedTeam.Currency.Should().Be("USD");
    }

    [Fact]
    public async Task UpdateAsync_WithUpdatedBudget_ShouldPersistChanges()
    {
        // Arrange
        var team = Team.Create(
            "Update Test Team",
            "City",
            "Country",
            2000,
            "Stadium",
            Money.Create(1000000m, "USD"));

        await _repository.AddAsync(team);
        await _context.SaveChangesAsync();

        // Act
        team.UpdateBudget(Money.Create(2000000m, "USD"));
        await _repository.UpdateAsync(team);
        await _context.SaveChangesAsync();

        // Assert
        var updatedTeam = await _context.Teams
            .FirstOrDefaultAsync(t => t.Id == team.Id.Value);
        updatedTeam.Should().NotBeNull();
        updatedTeam!.Budget.Should().Be(2000000m);
    }
}