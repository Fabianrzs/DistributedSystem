using TryAdminBack.Application.UseCase.Auth.RefreshToken;

namespace Authentications.Application.Authentications.RefreshToken;

public sealed record RefreshTokenCommand() : ICommand<RefreshTokenDto>;
