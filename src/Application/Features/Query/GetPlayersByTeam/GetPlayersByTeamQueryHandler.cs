namespace Application.Features.Query.GetPlayersByTeam;

using MediatR;
using Common.Interfaces;
using Common.Models;
using Common.Paging;
using Domain.Common;
using Domain.ValueObjects;

public class GetPlayersByTeamQueryHandler 
    : IRequestHandler<GetPlayersByTeamQuery, Result<PagedResult<PlayerResponse>>>
{
    private readonly IPlayerRepository _playerRepository;

    public GetPlayersByTeamQueryHandler(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }

    public async Task<Result<PagedResult<PlayerResponse>>> Handle(
        GetPlayersByTeamQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _playerRepository.GetPlayersByTeamPagedAsync(
            TeamId.Create(request.TeamId),
            request.PageNumber,
            request.PageSize,
            request.SearchTerm,
            request.SortBy,
            request.SortDescending,
            cancellationToken);

        return Result<PagedResult<PlayerResponse>>.Success(result);
    }
}