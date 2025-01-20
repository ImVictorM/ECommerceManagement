using Application.Common.Security.Authorization.Policies;
using Application.Common.Security.Authorization.Roles;

using SharedKernel.Errors;

namespace Application.Common.Security.Authorization;

/// <summary>
/// Attribute used to authorize requests.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class AuthorizeAttribute : Attribute
{
    /// <summary>
    /// Gets the authorization required role.
    /// </summary>
    public string? RoleName { get; }

    /// <summary>
    /// Gets the authorization policy type.
    /// </summary>
    public Type? PolicyType { get; }

    internal AuthorizeAttribute(string? roleName = null, Type? policyType = null)
    {
        RoleName = roleName;
        PolicyType = policyType;

        if (policyType != null && !typeof(IPolicy).IsAssignableFrom(policyType))
        {
            throw new ArgumentException(
                $"The provided policy type must implement {nameof(IPolicy)}.",
                nameof(policyType)
            );
        }

        if (roleName != null)
        {
            try
            {
                RoleUtils.FromDisplayName(roleName);
            }
            catch (InvalidParseException)
            {
                throw new ArgumentException($"The provided role name is incorrect: {roleName}.", nameof(roleName));
            }
        }
    }
}
