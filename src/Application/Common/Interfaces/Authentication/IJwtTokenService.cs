using Domain.RoleAggregate;
using Domain.UserAggregate;

namespace Application.Common.Interfaces.Authentication;

/// <summary>
/// Service to generate JWT authentication tokens.
/// </summary>
public interface IJwtTokenService
{
    /// <summary>
    /// Generates an authentication token for the given user.
    /// </summary>
    /// <param name="user">The user utilized for generating the token.</param>
    /// <param name="roles">The user roles.</param>
    /// <returns>An authentication token.</returns>
    string GenerateToken(User user, IReadOnlyList<Role> roles);
}
