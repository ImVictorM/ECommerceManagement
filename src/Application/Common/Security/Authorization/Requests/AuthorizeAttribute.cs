using Application.Common.Security.Authorization.Policies;

using SharedKernel.Models;
using SharedKernel.ValueObjects;

using System.Reflection;

namespace Application.Common.Security.Authorization.Requests;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
internal sealed class AuthorizeAttribute : Attribute
{
    /// <summary>
    /// Gets the authorization required role.
    /// </summary>
    public string? RoleName { get; }

    /// <summary>
    /// Gets the authorization policy type.
    /// </summary>
    public Type? PolicyType { get; }

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
            throw new ArgumentException(
                $"The provided role name is incorrect: {roleName}", nameof(roleName)
            );
        }
    }

    /// <summary>
    /// Extracts authorization metadata from the attributes of a request type.
    /// </summary>
    /// <param name="requestType">The request type.</param>
    /// <returns>The authorization metadata.</returns>
    public static AuthorizationMetadata GetAuthorizationMetadata(Type requestType)
    {
        var attributes = requestType
            .GetCustomAttributes<AuthorizeAttribute>()
            .ToList();

        var requiredRoles = attributes
            .Where(attr => !string.IsNullOrWhiteSpace(attr.RoleName))
            .Select(attr => BaseEnumeration.FromDisplayName<Role>(attr.RoleName!))
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
            .Any(i =>
                i.IsGenericType
                && i.GetGenericTypeDefinition() == typeof(IPolicy<>)
            );
    }

    private static bool IsExistingRole(string roleName)
    {
        try
        {
            BaseEnumeration.FromDisplayName<Role>(roleName);

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
