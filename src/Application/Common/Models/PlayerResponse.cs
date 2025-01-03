namespace Application.Common.Models;

using Domain.Enum;

public record PlayerResponse
{
    public Guid Id { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public DateOnly DateOfBirth { get; init; }
    public int Age { get; init; }
    public decimal Height { get; init; }
    public Position Position { get; init; }
    public decimal MarketValue { get; init; }
    
    public record ContractInfo
    {
        public DateTime StartDate { get; init; }
        public DateTime? EndDate { get; init; }
        public decimal Salary { get; init; }
        public string Currency { get; init; }
        public bool IsActive { get; init; }
    }

    public ContractInfo? CurrentContract { get; init; }
    public PlayerResponse(
        Guid id,
        string firstName,
        string lastName,
        DateOnly dateOfBirth,
        decimal height,
        Position position,
        decimal marketValue)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        Age = CalculateAge(dateOfBirth);
        Height = height;
        Position = position;
        MarketValue = marketValue;
        // CurrentTeamId = currentTeamId;
    }

    private static int CalculateAge(DateOnly dateOfBirth)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var age = today.Year - dateOfBirth.Year;
        if (dateOfBirth > today.AddYears(-age)) age--;
        return age;
    }

}