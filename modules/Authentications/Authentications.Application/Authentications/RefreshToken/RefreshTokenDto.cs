namespace TryAdminBack.Application.UseCase.Auth.RefreshToken;
public sealed record RefreshTokenDto(
    string AccessToken,
    string RefreshToken
);
