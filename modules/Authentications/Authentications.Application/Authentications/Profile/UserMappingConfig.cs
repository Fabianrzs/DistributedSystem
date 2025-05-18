using Authentications.Application.Authentications.Dtos;
namespace Authentications.Application.Authentications.Profile;
public class UserMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<User, UserDto>()
            .Map(dest => dest.Roles, src => src.UserRoles.Select(ur => ur.Role.Name).ToList());
    }
}
