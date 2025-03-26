using SharedKernel.ValueObjects;

namespace Application.Common.Security.Identity;

/// <summary>
/// Represents an identity user.
/// </summary>
/// <param name="Id">The user identifier.</param>
/// <param name="Roles">The user roles.</param>
public record IdentityUser(
    string Id,
    IReadOnlyList<Role> Roles
);
