namespace Application.Players.Commands.UpdatePlayer;

using Common.Interfaces;
using Domain.Common;
using Domain.ValueObjects;
using Features.Command.UpdatePlayer;
using MediatR;

public class UpdatePlayerCommandHandler : IRequestHandler<UpdatePlayerCommand, Result>
{
    private readonly IPlayerRepository _playerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePlayerCommandHandler(
        IPlayerRepository playerRepository,
        IUnitOfWork unitOfWork)
    {
        _playerRepository = playerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        UpdatePlayerCommand request,
        CancellationToken cancellationToken)
    {
        var player = await _playerRepository.GetPlayerByIdAsync(PlayerId.Create(request.PlayerId));
        if (player is null)
            return Result.Failure(Error.NotFound("Player not found"));

        if (request.Height.HasValue)
            player.UpdateHeight(Height.Create(request.Height.Value));

        if (request.Position.HasValue)
            player.UpdatePosition(request.Position.Value);

        if (!string.IsNullOrWhiteSpace(request.Email) || !string.IsNullOrWhiteSpace(request.Phone))
            player.UpdateContactInfo(request.Email ?? player.GetEmail(), request.Phone ?? player.GetPhone());

        if (request.MarketValue.HasValue)
        {
            var money = Money.Create(request.MarketValue.Value, "EUR");
            player.UpdateMarketValue(money);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}