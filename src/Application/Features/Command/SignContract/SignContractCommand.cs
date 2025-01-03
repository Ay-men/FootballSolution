namespace Application.Features.Command.SignContract;

using Domain.Common;
using MediatR;

public record SignContractCommand(
    Guid PlayerId,
    Guid TeamId,
    DateTime StartDate,
    DateTime? EndDate,
    decimal Salary,
    string Currency) : IRequest<Result<Guid>>;