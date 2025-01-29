using System.Reflection;
using Application.Common.Security.Authorization.Policies;
using Application.Common.Security.Authorization.Roles;

namespace Application.Common.Security.Authorization.Requests;

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

    /// <summary>
    /// Initiates a new instance of the <see cref="AuthorizeAttribute"/> class.
    /// </summary>
    /// <param name="roleName">The required role name.</param>
    /// <param name="policyType">The required police type.</param>
    /// <exception cref="ArgumentException">Thrown when any of the arguments are invalid.</exception>
    public AuthorizeAttribute(string? roleName = null, Type? policyType = null)
    {
        RoleName = roleName;
        PolicyType = policyType;

        if (policyType != null && !IsPolice(policyType))
        {
            throw new ArgumentException(
                $"The provided policy type must implement {typeof(IPolicy<>).Name}",
                nameof(policyType)
            );
        }

        if (roleName != null && !IsExistingRole(roleName))
        {
            throw new ArgumentException($"The provided role name is incorrect: {roleName}", nameof(roleName));
        }
    }

    /// <summary>
    /// Extracts authorization metadata from the attributes of a request type.
    /// </summary>
    /// <param name="requestType">The request type.</param>
    /// <returns>The authorization metadata.</returns>
    public static AuthorizationMetadata GetAuthorizationMetadata(Type requestType)
    {
        var attributes = requestType.GetCustomAttributes<AuthorizeAttribute>().ToList();

        var requiredRoles = attributes
            .Where(attr => !string.IsNullOrWhiteSpace(attr.RoleName))
            .Select(attr => attr.RoleName!)
            .ToList();

        var requiredPolicies = attributes
            .Where(attr => attr.PolicyType != null)
            .Select(attr => attr.PolicyType!)
            .ToList();

        return new AuthorizationMetadata(requiredRoles, requiredPolicies);
    }

    private static bool IsPolice(Type type)
    {
        return type
            .GetInterfaces()
            .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IPolicy<>));
    }

    private static bool IsExistingRole(string roleName)
    {
        try
        {
            RoleUtils.FromDisplayName(roleName);

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
