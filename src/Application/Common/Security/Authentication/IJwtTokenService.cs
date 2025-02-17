using Application.Common.Security.Identity;

namespace Application.Common.Security.Authentication;

/// <summary>
/// Service to generate JWT authentication tokens.
/// </summary>
public interface IJwtTokenService
{
    /// <summary>
    /// Generates an authentication token for the given user.
    /// </summary>
    /// <param name="user">The user utilized for generating the token.</param>
    /// <returns>An authentication token.</returns>
    string GenerateToken(IdentityUser user);
}
