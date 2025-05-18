using Domain.Abstractions.Entities;

namespace Authentications.Domain.Modules.Entities;
public class Session : Entity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
