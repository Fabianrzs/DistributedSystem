namespace Domain.Abstractions.Events;
public class DomainEvents : IDomainEvent
{
    private readonly List<IDomainEvent> domainEvents = [];
    public List<IDomainEvent> Events => [.. domainEvents];
    public void ClearDomainEvents()
    {
        domainEvents.Clear();
    }

    public void Raise(IDomainEvent domainEvent)
    {
        domainEvents.Add(domainEvent);
    }
}
