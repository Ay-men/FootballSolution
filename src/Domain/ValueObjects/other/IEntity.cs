using Domain.Events;

namespace Domain.Entities.Common;

public interface IEntity
{
  IReadOnlyList<IDomainEvent> DomainEvents { get; }
  void ClearDomainEvents();
}