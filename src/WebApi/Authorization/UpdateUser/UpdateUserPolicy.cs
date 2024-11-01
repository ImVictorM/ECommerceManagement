using Microsoft.AspNetCore.Authorization;
using WebApi.Common.Interfaces;

namespace WebApi.Authorization.UpdateUser;

/// <summary>
/// Defines a policy to update users.
/// </summary>
public class UpdateUserPolicy : IAuthorizationPolicy
{
    /// <summary>
    /// The policy name.
    /// </summary>
    public const string Name = "UpdateUser";

    /// <inheritdoc/>
    public void ConfigurePolicy(AuthorizationOptions options)
    {
        options.AddPolicy(Name, policy => policy.Requirements.Add(new UpdateUserRequirement()));
    }
}
