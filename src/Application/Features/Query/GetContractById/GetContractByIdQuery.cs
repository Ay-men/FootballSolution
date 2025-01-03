namespace Application.Features.Query.GetContractById;

using Common.Models;
using Domain.Common;
using Domain.Interfaces;
using MediatR;
public record GetContractByIdQuery : IRequest<Result<SignContractResponse>>, ICacheable
{
    public GetContractByIdQuery(Guid contractId)
    {
        ContractId = contractId;
    }
    
    public Guid ContractId { get; }
    public bool BypassCache => false;
    public string CacheKey => $"contract:{ContractId}";
    public int SlidingExpirationInMinutes => 30;
    public int AbsoluteExpirationInMinutes => 60;
}