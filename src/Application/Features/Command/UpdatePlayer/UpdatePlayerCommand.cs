namespace Application.Features.Command.UpdatePlayer;

using Domain.Common;
using Domain.Enum;
using MediatR;

public record UpdatePlayerCommand(
    Guid PlayerId,
    decimal? Height,
    Position? Position,
    string? Email,
    string? Phone,
    decimal? MarketValue) : IRequest<Result>;