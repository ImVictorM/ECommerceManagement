using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using SharedKernel.Authorization;

namespace WebApi.Common.Extensions;

/// <summary>
/// Extension methods for the <see cref="ClaimsPrincipal"/> object.
/// </summary>
public static class ClaimsPrincipalExtensions
{
    /// <summary>
    /// Checks if the current user claims have the admin role.
    /// </summary>
    /// <param name="userClaims">The current claims principal.</param>
    /// <returns>A bool value indicating if the user is an admin or not.</returns>
    public static bool IsAdmin(this ClaimsPrincipal userClaims)
    {
        var roles = userClaims
            .FindAll(ClaimTypes.Role)
            .Select(claim => claim.Value);

        return Role.HasAdminRole(roles);
    }

    /// <summary>
    /// Retrieves the user identifier.
    /// </summary>
    /// <param name="userClaims">The current claims principal.</param>
    /// <returns>The user identifier.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the identifier could not be found using the <see cref="JwtRegisteredClaimNames.Sub"/> claim type.</exception>
    public static string GetId(this ClaimsPrincipal userClaims)
    {
        return userClaims
             .FindFirstValue(JwtRegisteredClaimNames.Sub) ??
             throw new InvalidOperationException("There was an error when trying to get the authenticated user identifier");
    }
}
