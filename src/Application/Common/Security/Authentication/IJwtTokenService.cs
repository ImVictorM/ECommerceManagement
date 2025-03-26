using Application.Common.Security.Identity;

namespace Application.Common.Security.Authentication;

/// <summary>
/// Defines a service to generate JWT authentication tokens.
/// </summary>
public interface IJwtTokenService
{
    /// <summary>
    /// Generates an authentication token for the given user.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <returns>The authentication token.</returns>
    string GenerateToken(IdentityUser user);
}
