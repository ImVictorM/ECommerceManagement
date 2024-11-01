using Microsoft.AspNetCore.Authorization;

namespace WebApi.Common.Interfaces;

/// <summary>
/// Defines a contract for authorization policies.
/// </summary>
public interface IAuthorizationPolicy
{
    /// <summary>
    /// Configures the policy.
    /// </summary>
    /// <param name="options">The authorization options to configure the policy.</param>
    void ConfigurePolicy(AuthorizationOptions options);
}
