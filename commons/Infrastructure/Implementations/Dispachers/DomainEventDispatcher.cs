using Application.Abstractions.Dispachers;
using Domain.Abstractions.Events;
using MassTransit;

namespace Infrastructure.Implementations.Dispachers;

public class DomainEventDispatcher(IBus bus)
    : IDomainEventDispatcher
{
    public async Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents)
    {
        foreach (IDomainEvent domainEvent in domainEvents)
        {
            await bus.Publish(domainEvent);
        }
    }
}
