namespace Domain.Entities.Common;

using MediatR;

public interface IDomainEvent : INotification
{
    Guid Id { get; }
    DateTime OccurredOn { get; }
}