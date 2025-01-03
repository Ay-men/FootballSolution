using Domain.Events;

namespace Domain.Entities.Common;
public abstract class DomainEvent : IDomainEvent
{
  public Guid Id { get; }
  public DateTime OccurredOn { get; }

  protected DomainEvent()
  {
    Id = Guid.NewGuid();
    OccurredOn = DateTime.UtcNow;
  }
}
// public static class DomainEvents
// {
//   private static readonly List<IDomainEvent> _events = new();
//
//   public static IReadOnlyList<IDomainEvent> GetAll() => _events.AsReadOnly();
//
//   public static void Add(IDomainEvent eventItem)
//   {
//     _events.Add(eventItem);
//   }
//
//   public static void Clear()
//   {
//     _events.Clear();
//   }
// }