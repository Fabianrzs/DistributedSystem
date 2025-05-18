using Domain.Abstractions.Errors;

namespace Authentications.Domain.Modules.Errors;

public static class AuthenticationErrors
{
    public static Error InvalidCredentials() => Error.Unauthorized(
        "Authentication.InvalidCredentials",
        "Las credenciales proporcionadas son incorrectas.");

    public static Error UserNotFound(Guid userId) => Error.NotFound(
        "Authentication.UserNotFound",
        $"No se encontró un usuario con el Id = '{userId}'.");

    public static Error SessionNotFound(Guid sessionId) => Error.Unauthorized(
        "Authentication.SessionNotFound",
        $"No se encontró una sesión activa con el Id = '{sessionId}'.");

    public static Error SessionExpired(Guid sessionId) => Error.Unauthorized(
        "Authentication.SessionExpired",
        $"La sesión con el Id = '{sessionId}' ha expirado.");

    public static Error SessionInactive(Guid sessionId) => Error.Unauthorized(
        "Authentication.SessionInactive",
        $"La sesión con el Id = '{sessionId}' está inactiva.");

    public static Error TokenInvalid() => Error.Problem(
        "Authentication.TokenInvalid",
        "El token de autenticación es inválido.");

    public static Error GenerateTokenInvalid() => Error.Problem(
        "Authentication.TokenInvalid",
        "El token de autenticación generado no fue valido.");

    public static Error TokenExpired() => Error.Problem(
        "Authentication.TokenExpired",
        "El token de autenticación ha expirado.");


    public static Error UserAlreadyExists(string userName) => Error.Conflict(
        "Authentication.UserAlreadyExists",
        $"Ya existe un usuario registrado con el correo '{userName}'.");
}
