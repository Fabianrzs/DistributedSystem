using Authentications.Application.Authentications.Dtos;

namespace Authentications.Application.Abstractions.Security;

public interface ITokenProvider
{
    /// <summary>
    /// Genera un Access Token para un usuario.
    /// </summary>
    string GenerateAccessToken(Guid sessionId, UserDto user);

    /// <summary>
    /// Genera un Refresh Token seguro.
    /// </summary>
    string GenerateRefreshToken(Guid sessionId, Guid userId);
}
