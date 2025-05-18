using Authentications.Application.Authentications.Dtos;

namespace Authentications.Application.Authentications.SignUp;
public sealed record SignUpCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName
) : ICommand<UserResultDto>;
