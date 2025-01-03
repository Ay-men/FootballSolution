namespace Application.Features.Command.SignContract;

using Common.Interfaces;
using Domain.Common;
using Domain.ValueObjects;
using MediatR;

public class SignContractCommandHandler : IRequestHandler<SignContractCommand, Result<Guid>>
{
    private readonly IPlayerRepository _playerRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly IContractRepository _contractRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SignContractCommandHandler(
        IPlayerRepository playerRepository,
        ITeamRepository teamRepository,
        IContractRepository contractRepository,
        IUnitOfWork unitOfWork)
    {
        _playerRepository = playerRepository;
        _teamRepository = teamRepository;
        _contractRepository = contractRepository;
        _unitOfWork = unitOfWork;

    }
    public async Task<Result<Guid>> Handle(
        SignContractCommand request,
        CancellationToken cancellationToken)
    {
        var player = await _playerRepository.GetPlayerByIdAsync(PlayerId.Create(request.PlayerId));

        if (player is null)
            return Result<Guid>.Failure(Error.NotFound("Player not found"));

        var team = await _teamRepository.GetByIdAsync(TeamId.Create(request.TeamId));
        if (team is null)
            return Result<Guid>.Failure(Error.NotFound("Team not found"));

        var contractDetails = new ContractDetails(
            request.StartDate,
            Money.Create(request.Salary, request.Currency),
            request.EndDate);

        var newContract = player.SignContract(team, contractDetails);

        if (newContract != null && newContract.IsSuccess)
        {
            await _contractRepository.AddAsync(newContract.Value, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
     
        return Result<Guid>.Success(player.Id.Value);
    }
}