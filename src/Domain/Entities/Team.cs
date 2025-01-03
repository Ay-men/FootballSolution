namespace Domain.Entities;

using Domain.ValueObjects;
using Events;
using Exceptions;
using Primitives;
public class Team : AggregateRoot<TeamId>
{
    private readonly List<Contract> _contracts = [];
    
    public IReadOnlyList<Contract> Contracts => _contracts.AsReadOnly();
    public string Name { get; private set; }
    public string City { get; private set; }
    public string Country { get; private set; }
    public int FoundedYear { get; private set; }
    public string Stadium { get; private set; }
    public Money Budget { get; private set; }
    
    protected Team(TeamId id):base(id)
    {
    }
    
    private Team(
        TeamId id,
        string name, 
        string city, 
        string country, 
        int foundedYear, 
        string stadium,
        Money initialBudget) : base(id)
    {
        Name = name;
        City = city;
        Country = country;
        FoundedYear = foundedYear;
        Stadium = stadium;
        Budget = initialBudget;
    }
    
    public static Team Create(
        string name, 
        string city, 
        string country, 
        int foundedYear, 
        string stadium,
        Money initialBudget)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Team name cannot be empty");
            
        var team = new Team(
            TeamId.CreateUnique(),
            name,
            city,
            country,
            foundedYear,
            stadium,
            initialBudget);
        
        team.AddDomainEvent(new TeamCreatedEvent(team.Id));
        return team;
    }
    public void UpdateDetails(
        string? city,
        string? country,
        int? foundedYear,
        string? stadium)
    {
        City = city;
        Country = country;
        FoundedYear = foundedYear.Value;
        Stadium = stadium;
        Update();
    }
    public void OfferContract(Player player, ContractDetails details)
    {
        if (details.Salary.Amount > Budget.Amount)
            throw new DomainException("Contract salary exceeds team budget");
            
        var contract = Contract.Create(player, this, details);
        _contracts.Add(contract);
        
        AddDomainEvent(new TeamOfferedContractEvent(Id, player.Id, details));
    }
    
    public void UpdateBudget(Money newBudget)
    {
        if (newBudget.Currency != Budget.Currency)
            throw new DomainException("Cannot change budget currency");
            
        Budget = newBudget;
        AddDomainEvent(new TeamBudgetUpdatedEvent(Id, newBudget));
    }
}