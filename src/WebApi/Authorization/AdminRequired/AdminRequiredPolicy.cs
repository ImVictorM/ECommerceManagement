using Microsoft.AspNetCore.Authorization;
using SharedKernel.Authorization;
using WebApi.Common.Interfaces;

namespace WebApi.Authorization.AdminRequired;

/// <summary>
/// Defines a policy that requires admin role.
/// </summary>
public class AdminRequiredPolicy : IAuthorizationPolicy
{
    /// <summary>
    /// The policy name.
    /// </summary>
    public const string Name = nameof(AdminRequiredPolicy);

    /// <inheritdoc/>
    public void ConfigurePolicy(AuthorizationOptions options)
    {
        options.AddPolicy(Name, policy => policy.RequireRole(Role.Admin.Name));
    }
}
