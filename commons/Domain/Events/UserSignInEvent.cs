namespace Domain.Events;

public record UserSignInEvent(Guid UserId, string Email, string Name)
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}
