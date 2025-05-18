using Domain.Abstractions.Entities;

namespace Authentications.Domain.Modules.Entities;
public class User : Entity
{
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public ICollection<UserRole> UserRoles { get; set; } = [];
    public ICollection<Session> Sessions { get; set; } = [];
}
