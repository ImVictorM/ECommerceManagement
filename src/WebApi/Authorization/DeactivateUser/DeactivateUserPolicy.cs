using Microsoft.AspNetCore.Authorization;
using WebApi.Common.Interfaces;

namespace WebApi.Authorization.DeactivateUser;

/// <summary>
/// Defines a policy to deactivate users.
/// </summary>
public class DeactivateUserPolicy : IAuthorizationPolicy
{
    /// <summary>
    /// The policy name.
    /// </summary>
    public const string Name = nameof(DeactivateUserPolicy);

    /// <inheritdoc/>
    public void ConfigurePolicy(AuthorizationOptions options)
    {
        options.AddPolicy(Name, policy => policy.Requirements.Add(new DeactivateUserRequirement()));
    }
}
