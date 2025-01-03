namespace Application.Features.Query.GetContractById;

using AutoMapper;
using Common.Interfaces;
using Common.Models;
using Domain.Common;
using MediatR;

public class GetContractByIdQueryHandler : IRequestHandler<GetContractByIdQuery, Result<SignContractResponse>>
{

     private readonly IContractRepository _contractRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly IMapper _mapper;

    public GetContractByIdQueryHandler(
        IContractRepository contractRepository,
        IPlayerRepository playerRepository,
        ITeamRepository teamRepository,
        IMapper mapper)
    {
        _contractRepository = contractRepository;
        _playerRepository = playerRepository;
        _teamRepository = teamRepository;
        _mapper = mapper;
    }

    public async Task<Result<SignContractResponse>> Handle(
        GetContractByIdQuery request,
        CancellationToken cancellationToken)
    {
        var contract = await _contractRepository.GetByIdAsync(request.ContractId, cancellationToken);

        if (contract is null)
        {
            return Result<SignContractResponse>.Failure(
                Error.NotFound($"Contract with ID {request.ContractId} not found"));
        }

        if (!contract.IsActive())
        {
            return Result<SignContractResponse>.Failure(
                Error.NotFound($"No active contract found for ID {request.ContractId}"));
        }

        var player = await _playerRepository.GetPlayerByIdAsync(contract.GetPlayerId(), cancellationToken);
        var team = await _teamRepository.GetByIdAsync(contract.GetTeamId(), cancellationToken);

        if (player is null)
        {
            return Result<SignContractResponse>.Failure(
                Error.NotFound($"Player not found for contract {request.ContractId}"));
        }

        if (team is null)
        {
            return Result<SignContractResponse>.Failure(
                Error.NotFound($"Team not found for contract {request.ContractId}"));
        }

        var response = new SignContractResponse
        {
            ContractId = contract.Id,
            SignedAt = contract.GetStartDate(),
            Contract = new ContractBasicInfo
            {
                PlayerId = contract.GetPlayerId().Value,
                PlayerName = $"{player.GetFullName()}",
                TeamId = contract.GetTeamId().Value,
                TeamName = team.Name,
                StartDate = contract.GetStartDate(),
                EndDate = contract.GetEndDate(),
                Salary = new MoneyInfo
                {
                    Amount = contract.GetSalary(),
                    Currency = contract.GetSalaryCurrency()
                }
            }
        };

        return Result<SignContractResponse>.Success(response);
    }
}
    