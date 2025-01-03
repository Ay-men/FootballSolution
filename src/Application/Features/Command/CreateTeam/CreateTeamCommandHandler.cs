using Domain.Entities.ValueObjects;
using Domain.Exceptions;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Command.CreateTeam;

using Common.Interfaces;
using Domain.Common;
using Domain.ValueObjects;

public class CreateTeamCommandHandler : IRequestHandler<CreateTeamCommand, Result<Guid>>
{
    private readonly ITeamRepository _teamRepository;
    private readonly ITeamUniquenessChecker _uniquenessChecker;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTeamCommandHandler(
        ITeamRepository teamRepository,
        ITeamUniquenessChecker uniquenessChecker,
        IUnitOfWork unitOfWork)
    {
        _teamRepository = teamRepository;
        _uniquenessChecker = uniquenessChecker;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(
        CreateTeamCommand request,
        CancellationToken cancellationToken)
    {
        var isUnique = await _uniquenessChecker.IsUnique(
            request.Name,
            request.City,
            request.Country,
            request.FoundedYear);

        if (!isUnique)
            return Result<Guid>.Failure(Error.Conflict("Team with these details already exists"));

        var team = Team.Create(
            request.Name,
            request.City,
            request.Country,
            request.FoundedYear,
            request.Stadium,
            Money.Create(request.InitialBudget, request.Currency));

        await _teamRepository.AddAsync(team, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(team.Id.Value);
        
    }
}