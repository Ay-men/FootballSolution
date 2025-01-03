namespace Application.Features.Query.GetTeamDetails;

using Common.Models;
using Domain.Common;
using Domain.Interfaces;
using Domain.ValueObjects;
using MediatR;

public class GetTeamDetailsQuery : IRequest<Result<TeamResponse>>, ICacheable
{
    public GetTeamDetailsQuery(Guid teamId)
    {
        TeamId = teamId;
    }
    public Guid TeamId { get; }
    public string CacheKey => $"team-details:{TeamId}";
    public bool BypassCache => false;

    public int SlidingExpirationInMinutes => 30;

    public int AbsoluteExpirationInMinutes => 60;
}