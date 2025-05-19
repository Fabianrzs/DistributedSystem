using Domain.Abstractions.Events;

namespace Application.Abstractions.Dispachers;

public interface IDomainEventDispatcher
{
    Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents);
}
