using System.Security.Claims;
using Application.Abstractions.Security;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Implementations.Security;

public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public Guid UserId => GetClaimValue(ClaimTypes.NameIdentifier) is string userId && Guid.TryParse(userId, out Guid guid) ? guid : Guid.Empty;
    public string Email => GetClaimValue(ClaimTypes.Name) ?? string.Empty;
    public Guid SessionId => GetClaimValue("SessionId") is string sessionId && Guid.TryParse(sessionId, out Guid guid) ? guid : Guid.Empty;
    public List<string> Roles => httpContextAccessor.HttpContext?.User?.Claims
        .Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList() ?? [];

    private string? GetClaimValue(string claimType) => httpContextAccessor.HttpContext?.User?.FindFirst(claimType)?.Value;

}
