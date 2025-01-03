namespace Application.Common.Models;

public record TeamResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string? City { get; init; }
    public string? Country { get; init; }
    public int? FoundedYear { get; init; }
    public string? Stadium { get; init; }
    public decimal Budget { get; init; }
    public string Currency { get; init; }

    public ICollection<PlayerResponse> ActivePlayers { get; init; } = new List<PlayerResponse>();

    public record Statistics
    {
        public int TotalPlayers { get; init; }
        public decimal TotalMarketValue { get; init; }
        public decimal AverageAge { get; init; }
        public int InternationalPlayers { get; init; }
    }

    public Statistics? TeamStatistics { get; init; }

    public TeamResponse(
        Guid id,
        string name,
        string? city,
        string? country,
        int? foundedYear,
        string? stadium,
        decimal budget,
        string currency)
    {
        Id = id;
        Name = name;
        City = city;
        Country = country;
        FoundedYear = foundedYear;
        Stadium = stadium;
        Budget = budget;
        Currency = currency;
    }

}