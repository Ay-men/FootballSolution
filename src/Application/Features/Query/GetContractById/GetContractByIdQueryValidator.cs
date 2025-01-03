namespace Application.Features.Query.GetContractById;

using FluentValidation;

public class GetContractByIdQueryValidator : AbstractValidator<GetContractByIdQuery>
{
    public GetContractByIdQueryValidator()
    {
        RuleFor(x => x.ContractId)
            .NotEmpty()
            .WithMessage("Contract ID is required")
            .Must(id => id != Guid.Empty)
            .WithMessage("Invalid Contract ID format");
    }
}