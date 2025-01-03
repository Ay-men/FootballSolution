namespace Application.Players.Commands.UpdatePlayer;

using Features.Command.UpdatePlayer;
using FluentValidation;

public class UpdatePlayerCommandValidator : AbstractValidator<UpdatePlayerCommand>
{
    public UpdatePlayerCommandValidator()
    {
        RuleFor(x => x.PlayerId)
            .NotEmpty();

        When(x => x.Height.HasValue, () =>
        {
            RuleFor(x => x.Height!.Value)
                .InclusiveBetween(1.40m, 2.50m);
        });

        When(x => !string.IsNullOrWhiteSpace(x.Email), () =>
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .MaximumLength(100);
        });

        When(x => !string.IsNullOrWhiteSpace(x.Phone), () =>
        {
            RuleFor(x => x.Phone)
                .MaximumLength(20);
        });

        When(x => x.MarketValue.HasValue, () =>
        {
            RuleFor(x => x.MarketValue!.Value)
                .GreaterThanOrEqualTo(0);
        });
    }
}