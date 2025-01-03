namespace Infrastructure.Persistence.Mapping;

using AutoMapper;
using Domain.Entities;
using Domain.Entities.ValueObjects;
using Domain.ValueObjects;
using Domain.Enum;
using Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<TeamId, Guid>().ConvertUsing(id => id.Value);
        CreateMap<Guid, TeamId>().ConvertUsing(guid => TeamId.Create(guid));

        CreateMap<PlayerId, Guid>().ConvertUsing(id => id.Value);
        CreateMap<Guid, PlayerId>().ConvertUsing(guid => PlayerId.Create(guid));

        CreateMap<Money, decimal>().ConvertUsing(money => money.Amount);
        CreateMap<decimal, Money>().ConvertUsing(amount => Money.Create(amount, "USD")); 

        CreateMap<Player, PlayerEntity>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.GetFirstName()))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.GetLastName()))
            .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.GetDateOfBirth()))
            .ForMember(dest => dest.Height, opt => opt.MapFrom(src => src.GetHeight().Value))
            .ForMember(dest => dest.MarketValue, opt => opt.MapFrom(src => src.GetMarketValue().Value.Amount))
            .ForMember(dest => dest.Position, opt => opt.MapFrom(src => (int)src.GetPosition()))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.GetEmail()))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.GetPhone()))
            .ForMember(dest => dest.Nationality, opt => opt.MapFrom(src => src.GetNationality()))
            .ForMember(dest => dest.PassportNumber, opt => opt.MapFrom(src => src.GetPassportNumber()));

        CreateMap<Contract, PlayerTeamEntity>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid())) // Assuming contract ID isn't set in domain
            .ForMember(dest => dest.PlayerId, opt => opt.MapFrom(src => src.GetPlayerId().Value))
            .ForMember(dest => dest.TeamId, opt => opt.MapFrom(src => src.GetTeamId().Value))
            .ForMember(dest => dest.Salary, opt => opt.MapFrom(src => src.GetSalary()))
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.GetStartDate()))
            .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.GetEndDate()))
            .ForMember(dest => dest.SalaryCurrency, opt => opt.MapFrom(src => src.GetSalaryCurrency()));

        CreateMap<Team, TeamEntity>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country))
            .ForMember(dest => dest.FoundedYear, opt => opt.MapFrom(src => src.FoundedYear))
            .ForMember(dest => dest.Stadium, opt => opt.MapFrom(src => src.Stadium))
            .ForMember(dest => dest.Budget, opt => opt.MapFrom(src => src.Budget.Amount))
            .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Budget.Currency));

        CreateMap<PlayerEntity, Player>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => PlayerId.Create(src.Id)))
            .ConstructUsing((src, ctx) =>
            {
                var money = Money.Create(src.MarketValue, "EUR");
                var personalInfo = PersonalInfo.Create(
                    src.FirstName,
                    src.LastName,
                    DateOnly.FromDateTime(src.DateOfBirth));

                var player = Player.Create(
                    personalInfo,
                    Height.Create(src.Height),
                    (Position)src.Position,
                    MarketValue.Create(money),
                    src.Email,
                    src.Phone,
                    src.Nationality,
                    src.PassportNumber);

                return player.Value;

            });
        CreateMap<TeamEntity, Team>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => TeamId.Create(src.Id)))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country))
            .ForMember(dest => dest.Budget, opt => opt.MapFrom(src => src.Budget))
            .ForMember(dest => dest.Stadium, opt => opt.MapFrom(src => src.Stadium));

        CreateMap<PlayerTeamEntity, Contract>()
            .ConstructUsing((src, ctx) => Contract.Create(
                ctx.Mapper.Map<Player>(src.PlayerEntity),
                ctx.Mapper.Map<Team>(src.TeamEntity),
                new ContractDetails(
                    src.StartDate,
                    Money.Create(src.Salary, src.SalaryCurrency),
                    src.EndDate)));
    }
}