using Domain.Abstractions.Entities;

namespace Authentications.Domain.Modules.Entities;
public class Role : Entity
{
    public string Name { get; set; } = string.Empty;
    public ICollection<UserRole> UserRoles { get; set; } = [];
}
