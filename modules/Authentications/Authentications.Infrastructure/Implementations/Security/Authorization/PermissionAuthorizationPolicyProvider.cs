using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace TryAdminBack.Infrastructure.Implementations.Security.Authorization;

internal sealed class PermissionAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
{
    private readonly AuthorizationOptions authorizationOptions;

    public PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
        : base(options)
    {
        authorizationOptions = options.Value;
    }

    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        AuthorizationPolicy? policy = await base.GetPolicyAsync(policyName);

        if (policy is not null)
        {
            return policy;
        }

        AuthorizationPolicy permissionPolicy = new AuthorizationPolicyBuilder()
            .AddRequirements(new PermissionRequirement(policyName))
            .Build();

        authorizationOptions.AddPolicy(policyName, permissionPolicy);

        return permissionPolicy;
    }
}
