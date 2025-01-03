namespace Domain.Entities;

using Common;
using Domain.ValueObjects;
using Enum;
using Events;
using Exceptions;
using Primitives;
using ValueObjects;
public class Player : AggregateRoot<PlayerId>
{
    private readonly List<Contract> _contracts = new();
    private PersonalInfo _personalInfo;
    private Height _height;
    private Position _position;
    private MarketValue _marketValue;
    private string _nationality;
    private string _email;
    private string _phone;
    private string _passportNumber;

    private Player(
        PlayerId id,
        PersonalInfo personalInfo,
        Height height,
        Position position,
        MarketValue marketValue,
        string email,
        string phone,
        string nationality,
        string passportNumber) : base(id)
    {
        _personalInfo = personalInfo;
        _height = height;
        _position = position;
        _marketValue = marketValue;
        _email = email;
        _phone = phone;
        _nationality = nationality;
        _passportNumber = passportNumber;
    }

    public static Result<Player> Create(
        PersonalInfo personalInfo,
        Height height,
        Position position,
        MarketValue marketValue,
        string email,
        string phone,
        string nationality,
        string passportNumber)
    {
        if (string.IsNullOrWhiteSpace(nationality))
            return Result<Player>.Failure(Error.Validation("Nationality is required"));

        if (string.IsNullOrWhiteSpace(phone))
            return Result<Player>.Failure(Error.Validation("Phone is required"));

        if (string.IsNullOrWhiteSpace(passportNumber))
            return Result<Player>.Failure(Error.Validation("Passport number is required"));

        var player = new Player(
            PlayerId.CreateUnique(),
            personalInfo,
            height,
            position,
            marketValue,
            email,
            phone,
            nationality,
            passportNumber);

        player.AddDomainEvent(new PlayerCreatedEvent(player.Id));
        return Result<Player>.Success(player);
    }

    public Result UpdateHeight(Height newHeight)
    {
        _height = newHeight;
        Update();
        return Result.Success();
    }

    public Result UpdatePosition(Position newPosition)
    {
        _position = newPosition;
        Update();
        return Result.Success();
    }

    public Result UpdateContactInfo(string email, string phone)
    {
        if (string.IsNullOrWhiteSpace(email))
            return Result.Failure(Error.Validation("Email cannot be empty"));
            
        if (string.IsNullOrWhiteSpace(phone))
            return Result.Failure(Error.Validation("Phone cannot be empty"));

        _email = email;
        _phone = phone;
        Update();
        return Result.Success();
    }

    public Result<Contract> SignContract(Team team, ContractDetails details)
    {
        var tt = IsUnderContract();
        if (IsUnderContract())
            return Result<Contract>.Failure(Error.Validation("Player already has an active contract"));

        var contract = Contract.Create(this, team, details);
        _contracts.Add(contract);

        AddDomainEvent(new PlayerContractSignedEvent(Id, team.Id, details));
        return Result<Contract>.Success(contract);
    }

    public Result UpdateMarketValue(Money newValue)
    {
        var oldValue = _marketValue;
        _marketValue = MarketValue.Create(newValue);
        
        AddDomainEvent(new PlayerMarketValueUpdatedEvent(Id, _marketValue));
        return Result.Success();
    }
    
    public bool IsUnderContract()
    {
        var activeContract = GetActiveContract();
        
        var t = activeContract?.GetType() == typeof(Contract);
        var tt = (activeContract?.GetTeamId().Value != Guid.Empty && activeContract?.GetTeamId().Value != null) ||
                 (activeContract?.GetType() == typeof(Contract) && activeContract != null);
        return tt;
    }
    public TeamId? GetCurrentTeamId() => GetActiveContract()?.GetTeamId();
    public decimal GetCurrentSalary() => GetActiveContract()?.GetSalary() ?? 0;
    public string GetCurrentSalaryCurrency() => GetActiveContract()?.GetSalaryCurrency() ?? "";
    public DateTime? GetContractEndDate() => GetActiveContract()?.GetEndDate();
    public string GetFullName() => $"{_personalInfo.FirstName} {_personalInfo.LastName}";
    public string GetFirstName() => $"{_personalInfo.LastName}";
    public string GetLastName() => $"{_personalInfo.LastName}";
    public string GetDateOfBirth() => $"{_personalInfo.DateOfBirth}";
    public string GetNationality() => _nationality;
    public string GetEmail() => _email;
    public string GetPhone() => _phone;
    public string GetPassportNumber() => _passportNumber;
    public Height GetHeight() => _height;
    public Position GetPosition() => _position;
    public MarketValue GetMarketValue() => _marketValue;

    private Contract? GetActiveContract()
    {
        return _contracts.FirstOrDefault(c => c.IsActive());
 
    }

}