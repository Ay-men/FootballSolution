namespace FootballSolution.Tests;

using AutoMapper;
using FluentAssertions;
using global::Domain.ValueObjects;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Mapping;
using Infrastructure.Repositories;

[Collection("IntegrationTests")]
public class PlayerRepositoryTests : IClassFixture<DbContextFixture>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly PlayerRepository _repository;

    public PlayerRepositoryTests(DbContextFixture fixture)
    {
        _context = fixture.Context;
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = configuration.CreateMapper();
        _repository = new PlayerRepository(_context, _mapper);
    }

    [Fact]
    public async Task GetPlayerByIdAsync_WithNonexistentId_ReturnsNull()
    {
        // Arrange
        var nonexistentId = PlayerId.Create(Guid.NewGuid());

        // Act
        var result = await _repository.GetPlayerByIdAsync(nonexistentId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetPlayerByIdAsync_WithExistingPlayer_ReturnsMappedDomainEntity()
    {
        // Arrange
        var player = Helpers.CreateValidPlayerEntity();
        await _context.Players.AddAsync(player);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetPlayerByIdAsync(PlayerId.Create(player.Id));

        // Assert
        result.Should().NotBeNull();
        result!.GetFullName().Should().Be($"{player.FirstName} {player.LastName}");
        result.GetMarketValue().Value.Amount.Should().Be(player.MarketValue);
    }
}
