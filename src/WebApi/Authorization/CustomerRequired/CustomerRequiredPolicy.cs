using Microsoft.AspNetCore.Authorization;
using SharedKernel.Authorization;
using WebApi.Common.Interfaces;

namespace WebApi.Authorization.CustomerRequired;

/// <summary>
/// Defines a policy that requires the customer role.
/// </summary>
public class CustomerRequiredPolicy : IAuthorizationPolicy
{
    /// <summary>
    /// The policy name.
    /// </summary>
    public const string Name = nameof(CustomerRequiredPolicy);

    /// <inheritdoc/>
    public void ConfigurePolicy(AuthorizationOptions options)
    {
        options.AddPolicy(Name, policy => policy.RequireRole(Role.Customer.Name));
    }
}
