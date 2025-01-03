namespace Application.Features.Query.GetTeamDetails;

using MediatR;
using Common.Interfaces;
using Common.Models;
using Domain.Common;
using Domain.ValueObjects;

public class GetTeamDetailsQueryHandler : IRequestHandler<GetTeamDetailsQuery, Result<TeamResponse>>
{
    private readonly ITeamRepository _teamRepository;

    public GetTeamDetailsQueryHandler(ITeamRepository teamRepository)
    {
        _teamRepository = teamRepository;
    }

    public async Task<Result<TeamResponse>> Handle(
        GetTeamDetailsQuery request,
        CancellationToken cancellationToken)
    {
        var team = await _teamRepository.GetByIdAsync(TeamId.Create(request.TeamId), cancellationToken);
        if (team is null)
            return Result<TeamResponse>.Failure(Error.NotFound("Team not found"));

        var response = new TeamResponse(
            team.Id.Value,
            team.Name,
            team.City,
            team.Country,
            team.FoundedYear,
            team.Stadium,
            team.Budget.Amount,
            team.Budget.Currency);

        return Result<TeamResponse>.Success(response);
    }
}