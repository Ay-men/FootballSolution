namespace Application.Features.Command.SignContract;

using FluentValidation;

public class SignContractCommandValidator : AbstractValidator<SignContractCommand>
{
    public SignContractCommandValidator()
    {
        RuleFor(x => x.PlayerId)
            .NotEmpty();

        RuleFor(x => x.TeamId)
            .NotEmpty();

        RuleFor(x => x.StartDate)
            .NotEmpty()
            .GreaterThan(DateTime.UtcNow);

        RuleFor(x => x.EndDate)
            .Must((cmd, endDate) => 
                !endDate.HasValue || endDate.Value > cmd.StartDate)
            .WithMessage("End date must be after start date");

        RuleFor(x => x.Salary)
            .GreaterThan(0);

        RuleFor(x => x.Currency)
            .NotEmpty()
            .Length(3)
            .Must(c => c.All(char.IsUpper));
    }
}