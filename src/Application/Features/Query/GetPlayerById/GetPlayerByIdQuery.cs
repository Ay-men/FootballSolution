namespace Application.Features.Query.GetPlayerById;

using Common.Models;
using Domain.Common;
using Domain.Interfaces;
using Domain.ValueObjects;
using MediatR;

public record GetPlayerByIdQuery : IRequest<Result<PlayerResponse>>,ICacheable
{

     public GetPlayerByIdQuery(Guid playerId)
     {
          PlayerId = playerId;
     }
     public Guid PlayerId { get; }
     public bool BypassCache => false;
     public string CacheKey => $"player:{PlayerId}";

     public int SlidingExpirationInMinutes => 30;

     public int AbsoluteExpirationInMinutes => 60;
}