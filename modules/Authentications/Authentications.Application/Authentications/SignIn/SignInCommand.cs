using Authentications.Application.Authentications.Dtos;

namespace Authentications.Application.Authentications.SignIn;

public sealed record SignInCommand(
    string Email,
    string Password) : ICommand<UserResultDto>;
