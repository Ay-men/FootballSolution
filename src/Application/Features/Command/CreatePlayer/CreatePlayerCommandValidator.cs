namespace Application.Features.Command.CreatePlayer;

using FluentValidation;
public class CreatePlayerCommandValidator : AbstractValidator<CreatePlayerCommand>
{
  public CreatePlayerCommandValidator()
  {
    RuleFor(x => x.FirstName)
        .NotEmpty()
        .MaximumLength(100);

    RuleFor(x => x.LastName)
        .NotEmpty()
        .MaximumLength(100);

    RuleFor(x => x.DateOfBirth)
        .Must(dob =>
        {
          var age = DateTime.Today.Year - dob.Year;
          return age is >= 16 and <= 45;
        })
        .WithMessage("Player must be between 16 and 45 years old");

    RuleFor(x => x.Height)
        .InclusiveBetween(1.40m, 2.50m);

    RuleFor(x => x.MarketValue)
        .GreaterThan(0);

    RuleFor(x => x.Currency)
        .NotEmpty()
        .Length(3)
        .Must(c => c.All(char.IsUpper))
        .WithMessage("Currency must be a valid 3-letter ISO code");
  }
}