using Domain.Abstractions.Events;

namespace Domain.Events;

public record UserSignUpEvent(Guid UserId, string Email) : IDomainEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}
