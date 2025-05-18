using Domain.Abstractions.Entities;

namespace Authentications.Domain.Modules.Entities;
public class UserRole : Entity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public Guid RoleId { get; set; }
    public Role Role { get; set; } = null!;
}
