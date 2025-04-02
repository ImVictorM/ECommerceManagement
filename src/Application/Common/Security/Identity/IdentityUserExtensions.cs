using SharedKernel.ValueObjects;

namespace Application.Common.Security.Identity;

/// <summary>
/// Extension methods for the <see cref="IdentityUser"/> class.
/// </summary>
public static class IdentityUserExtensions
{
    /// <summary>
    /// Verifies if the current user is an administrator.
    /// </summary>
    /// <param name="user">The current user.</param>
    /// <returns>
    /// A bool value indicating if the current user is an administrator.
    /// </returns>
    public static bool IsAdmin(this IdentityUser user)
    {
        return user.Roles.Any(role => role == Role.Admin);
    }
}
