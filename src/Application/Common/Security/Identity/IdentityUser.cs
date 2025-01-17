namespace Application.Common.Security.Identity;

/// <summary>
/// Represents an identity user.
/// </summary>
/// <param name="Id">The user id.</param>
/// <param name="Roles">The user roles.</param>
public record IdentityUser(
    string Id,
    IReadOnlyList<string> Roles
);
