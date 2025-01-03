namespace Domain.Interfaces;

using MediatR;

public interface IDomainEvent: INotification
{
    public Guid Id { get; init; }
}