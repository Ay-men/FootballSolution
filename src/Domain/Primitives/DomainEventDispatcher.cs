namespace Domain.Primitives;

using Interfaces;
using Microsoft.Extensions.DependencyInjection;

public class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public async Task DispatchEventsAsync(IEnumerable<IDomainEvent> events)
    {
        foreach (var @event in events)
        {
            var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(@event.GetType());
            var handlers = _serviceProvider.GetServices(handlerType);

            foreach (dynamic handler in handlers) await handler.Handle((dynamic)@event);
        }
    }
}