namespace Domain.Events;

public record UserSignUpEvent(Guid UserId, string Email, string Name)
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}
