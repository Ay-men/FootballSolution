namespace Application.Features.Query.GetPlayersByTeam;

using Common.Models;
using Common.Paging;
using Domain.Common;
using Domain.Interfaces;
using Domain.ValueObjects;
using MediatR;

public record GetPlayersByTeamQuery(
    Guid TeamId,
    int PageNumber = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    string? SortBy = null,
    bool SortDescending = false) : IRequest<Result<PagedResult<PlayerResponse>>>, ICacheable
{

     public string CacheKey => $"players-by-team:{TeamId}";

     public bool BypassCache => false;

     public int SlidingExpirationInMinutes => 30;

     public int AbsoluteExpirationInMinutes => 60;
}
