using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Authentications.Application.Abstractions.Security;
using Authentications.Application.Authentications.Dtos;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Authentications.Infrastructure.Implementations.Security.Authentication;

public class TokenProvider(IConfiguration configuration) : ITokenProvider
{
    public string GenerateAccessToken(Guid sessionId, UserDto user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, user.Email),
            new(JwtRegisteredClaimNames.Name, user.FirstName),
            new(JwtRegisteredClaimNames.FamilyName, user.LastName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new("SessionId", sessionId.ToString())
        };

        user.Roles.ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));

        int expirationInMinutes = configuration.GetValue<int>("Jwt:AccessTokenExpiration");
        string secretKey = configuration["Jwt:AccessSecret"]!;
        return GenerateToken(claims, expirationInMinutes, secretKey);
    }

    /// <summary>
    /// Genera un Refresh Token que solo contiene el SessionId y UserId.
    /// </summary>
    public string GenerateRefreshToken(Guid sessionId, Guid userId)
    {
        var claims = new List<Claim>
        {
            new("SessionId", sessionId.ToString()),
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        int expirationInMinutes = configuration.GetValue<int>("Jwt:RefreshTokenExpiration");
        string secretKey = configuration["Jwt:RefreshSecret"]!;
        return GenerateToken(claims, expirationInMinutes, secretKey);
    }

    /// <summary>
    /// Método reutilizable para generar tokens.
    /// </summary>
    private string GenerateToken(List<Claim> claims, int expirationInMinutes, string secretKey)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(expirationInMinutes),
            SigningCredentials = credentials,
            Issuer = configuration["Jwt:Issuer"],
            Audience = configuration["Jwt:Audience"]
        };

        var handler = new JwtSecurityTokenHandler();
        SecurityToken token = handler.CreateToken(tokenDescriptor);
        return handler.WriteToken(token);
    }
}
