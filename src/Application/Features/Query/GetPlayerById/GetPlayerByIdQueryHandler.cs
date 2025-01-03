namespace Application.Features.Query.GetPlayerById;

using Common.Interfaces;
using Common.Models;
using Domain.Common;
using Domain.Exceptions;
using Domain.ValueObjects;
using MediatR;

public class GetPlayerByIdQueryHandler : IRequestHandler<GetPlayerByIdQuery, Result<PlayerResponse>>
{
    private readonly IPlayerRepository _playerRepository;

    public GetPlayerByIdQueryHandler(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }

    public async Task<Result<PlayerResponse>> Handle(
        GetPlayerByIdQuery request,
        CancellationToken cancellationToken)
    {
        var player = await _playerRepository.GetPlayerByIdAsync(PlayerId.Create(request.PlayerId));
        if (player is null)
            return Result<PlayerResponse>.Failure(Error.NotFound("Player not found"));

        var response = new PlayerResponse(
            player.Id.Value,
            player.GetFirstName(),
            player.GetLastName(),
            DateOnly.Parse( player.GetDateOfBirth()),
            player.GetHeight().Value,
            player.GetPosition(),
            player.GetMarketValue().Value.Amount);

        return Result<PlayerResponse>.Success(response);
    }
}

