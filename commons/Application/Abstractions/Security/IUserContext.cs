namespace Application.Abstractions.Security;

public interface IUserContext
{
    /// <summary>
    /// Identificador único del usuario.
    /// </summary>
    Guid UserId { get; }

    /// <summary>
    /// Email de usuario autenticado.
    /// </summary>
    string Email { get; }

    /// <summary>
    /// Identificador de la sesión actual.
    /// </summary>
    Guid SessionId { get; }

    /// <summary>
    /// Lista de roles del usuario autenticado.
    /// </summary>
    List<string> Roles { get; }
}
