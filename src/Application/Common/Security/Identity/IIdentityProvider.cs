namespace Application.Common.Security.Identity;

/// <summary>
/// Defines a contract for retrieving information about the current authenticated user.
/// </summary>
public interface IIdentityProvider
{
    /// <summary>
    /// Gets the details of the current authenticated user.
    /// </summary>
    /// <returns>
    /// A <see cref="IdentityUser"/> instance representing the authenticated user.
    /// </returns>
    IdentityUser GetCurrentUserIdentity();
}
