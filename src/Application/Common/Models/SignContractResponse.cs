namespace Application.Common.Models;

public record SignContractResponse
{
    public Guid ContractId { get; init; }
    public DateTime SignedAt { get; init; }
    public ContractBasicInfo Contract { get; init; } = null!;
}

public record ContractBasicInfo
{
    public Guid PlayerId { get; init; }
    public string PlayerName { get; init; } = null!;
    public Guid TeamId { get; init; }
    public string TeamName { get; init; } = null!;
    public DateTime StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public MoneyInfo Salary { get; init; } = null!;
}

public record MoneyInfo
{
    public decimal Amount { get; init; }
    public string Currency { get; init; } = null!;
}