
namespace Application.Features.Command.CreatePlayer;

using Domain.Exceptions;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.ValueObjects;
using Domain.Interfaces;
using MediatR;
using Common.Interfaces;
using Domain.Common;
using Domain.ValueObjects;
public class CreatePlayerCommandHandler
    : IRequestHandler<CreatePlayerCommand, Result<Guid>>
{
  private readonly IPlayerRepository _playerRepository;
  private readonly IUnitOfWork _unitOfWork;

  public CreatePlayerCommandHandler(IPlayerRepository playerRepository,IUnitOfWork unitOfWork)
  {
    _playerRepository = playerRepository;
    _unitOfWork = unitOfWork;
  }

  public async Task<Result<Guid>> Handle(
      CreatePlayerCommand request,
      CancellationToken cancellationToken)
  {
    var personalInfo = PersonalInfo.Create(
        request.FirstName,
        request.LastName,
        request.DateOfBirth);

    var height = Height.Create(request.Height);

    var marketValue = MarketValue.Create(
        Money.Create(request.MarketValue, request.Currency));

    var player = Player.Create(
        personalInfo,
        height,
        request.Position,
        marketValue,
        request.Email,
        request.Phone,
        request.Nationality,
        request.PassportNumber
        );

    await _playerRepository.AddAsync(player.Value, cancellationToken);
    await _unitOfWork.SaveChangesAsync(cancellationToken);

    return Result<Guid>.Success(player.Value.Id.Value);
  }
}