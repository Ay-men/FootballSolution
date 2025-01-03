namespace Application.Features.Command.CreateTeam;

using MediatR;
using Domain.Common;

public record CreateTeamCommand(
    string Name,
    string City,
    string Country,
    int FoundedYear,
    string Stadium,
    decimal InitialBudget,
    string Currency) : IRequest<Result<Guid>>;
