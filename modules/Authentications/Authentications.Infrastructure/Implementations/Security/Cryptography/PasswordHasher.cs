using Authentications.Application.Abstractions.Security;

namespace Authentications.Infrastructure.Implementations.Security.Cryptography;

public class PasswordHasher : IPasswordHasher
{
    private const int WorkFactor = 12; // Puedes ajustar el costo (10–14 comúnmente)

    /// <summary>
    /// Genera un hash de contraseña utilizando PBKDF2 con un salt único.
    /// </summary>
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, workFactor: WorkFactor);
    }

    /// <summary>
    /// Verifica si la contraseña proporcionada coincide con el hash almacenado.
    /// </summary>
    public bool VerifyPassword(string hashedPassword, string providedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);
    }
}
