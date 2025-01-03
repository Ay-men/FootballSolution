namespace Application.Features.Command.CreatePlayer;

using Domain.Common;
using Domain.Enum;
using MediatR;

public record CreatePlayerCommand(
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    decimal Height,
    Position Position,
    decimal MarketValue,
    string Currency,
    string Email,
    string Phone,
    string Nationality,
    string PassportNumber) : IRequest<Result<Guid>>;