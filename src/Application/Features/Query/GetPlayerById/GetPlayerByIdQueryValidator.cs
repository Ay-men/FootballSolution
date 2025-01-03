namespace Application.Features.Query.GetPlayerById;

using FluentValidation;

public class GetPlayerByIdQueryValidator : AbstractValidator<GetPlayerByIdQuery>
{
    public GetPlayerByIdQueryValidator()
    {
        RuleFor(x => x.PlayerId)
            .NotEmpty();
    }
}