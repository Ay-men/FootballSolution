namespace Application.Common.Models;

using Domain.Enum;


public class TransferResponse
{
    public Guid TransferId { get; set; }
    public DateTime TransferDate { get; set; }
    
    public PlayerTransferDetails Player { get; set; } = null!;
    
    public TeamTransferDetails FromTeam { get; set; } = null!;
    public TeamTransferDetails ToTeam { get; set; } = null!;
    
    public MoneyResponse TransferFee { get; set; } = null!;
    public ContractTransferDetails NewContract { get; set; } = null!;
    
    public string Status { get; set; } = null!;
    public DateTime CompletedAt { get; set; }
}

public class PlayerTransferDetails
{
    public Guid PlayerId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string FullName => $"{FirstName} {LastName}";
    public Position Position { get; set; }
    public int Age { get; set; }
    public string Nationality { get; set; } = null!;
    public MoneyResponse MarketValue { get; set; } = null!;
}

public class TeamTransferDetails
{
    public Guid TeamId { get; set; }
    public string Name { get; set; } = null!;
    public string? City { get; set; }
    public string? Country { get; set; }
    public MoneyResponse Budget { get; set; } = null!;
}

public class ContractTransferDetails
{
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int DurationInMonths => EndDate.HasValue 
        ? ((EndDate.Value.Year - StartDate.Year) * 12) + (EndDate.Value.Month - StartDate.Month)
        : -1;
    public MoneyResponse Salary { get; set; } = null!;
    public MoneyResponse? SigningBonus { get; set; }
    public List<ContractClauseResponse> SpecialClauses { get; set; } = new();
}

public class ContractClauseResponse
{
    public string Type { get; set; } = null!;
    public string Description { get; set; } = null!;
    public MoneyResponse? Value { get; set; }
}

public class MoneyResponse
{
    public decimal Amount { get; set; }
    public string Currency { get; set; } = null!;
    public string Value => $"{Amount:N2} {Currency}";
}