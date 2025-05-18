using Domain.Abstractions.Events;

namespace Domain.Abstractions.Entities;

public abstract class Entity : DomainEvents, IEntity<Guid>
{
    public Guid Id { get; set; }
    public bool IsActive { get; set; } = true;
    public void Active()
    {
        IsActive = true;
    }
    public void InActive()
    {
        IsActive = false;
    }
}
