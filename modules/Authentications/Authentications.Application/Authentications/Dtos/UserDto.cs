namespace Authentications.Application.Authentications.Dtos;
public sealed record UserResultDto(
    Guid SessionId,
    UserDto User,
    string AccessToken,
    string RefreshToken
);

public sealed record UserDto(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    List<string> Roles
);
