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

        int expirationInMinutes = configuration.GetValue<int>("SECURITY-JWT-ACCESS-EXPIRATION-IN-MINUTES");
        string secretKey = configuration["SECURITY-JWT-SECRET-ACCESS"]
            ?? throw new ArgumentException("SECURITY-JWT-SECRET-ACCESS is not configured.");

        return GenerateToken(claims, expirationInMinutes, secretKey);
    }

    public string GenerateRefreshToken(Guid sessionId, Guid userId)
    {
        var claims = new List<Claim>
        {
            new("SessionId", sessionId.ToString()),
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        int expirationInMinutes = configuration.GetValue<int>("SECURITY-JWT-REFRESH-EXPIRATION-IN-MINUTES");
        string secretKey = configuration["SECURITY-JWT-SECRET-REFRESH"]
            ?? throw new ArgumentException("SECURITY-JWT-SECRET-REFRESH is not configured.");

        return GenerateToken(claims, expirationInMinutes, secretKey);
    }

    private string GenerateToken(List<Claim> claims, int expirationInMinutes, string secretKey)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        string issuer = configuration["SECURITY-JWT-ISSUER"]
            ?? throw new ArgumentException("SECURITY-JWT-ISSUER is not configured.");

        string audience = configuration["SECURITY-JWT-AUDIENCE"]
            ?? throw new ArgumentException("SECURITY-JWT-AUDIENCE is not configured.");

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime  .UtcNow.AddMinutes(expirationInMinutes),
            SigningCredentials = credentials,
            Issuer = issuer,
            Audience = audience
        };

        var handler = new JwtSecurityTokenHandler();
        SecurityToken token = handler.CreateToken(tokenDescriptor);
        return handler.WriteToken(token);
    }
}
