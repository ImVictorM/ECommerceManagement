using Application.Common.Security.Identity;

using SharedKernel.Models;
using SharedKernel.ValueObjects;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Security.Identity;

/// <summary>
/// Represents an identity provider implementation
/// </summary>
public class IdentityProvider : IIdentityProvider
{
    private readonly HttpContext _httpContext;

    /// <summary>
    /// Initiates a new instance of the <see cref="IdentityProvider"/> class.
    /// </summary>
    /// <param name="httpContextAccessor">The http context accessor.</param>
    /// <exception cref="InvalidOperationException">Exception thrown when the http context is null.</exception>
    public IdentityProvider(IHttpContextAccessor httpContextAccessor)
    {
        if (httpContextAccessor.HttpContext == null)
        {
            throw new InvalidOperationException(
                "HttpContext is not available. " +
                "Ensure that IHttpContextAccessor is properly configured and the request context is valid."
            );
        }

        _httpContext = httpContextAccessor.HttpContext;
    }

    /// <inheritdoc/>
    public IdentityUser GetCurrentUserIdentity()
    {
        var id = GetSingleClaimValue(JwtRegisteredClaimNames.Sub);

        var roles = GetClaimValues(ClaimTypes.Role)
            .Select(BaseEnumeration.FromDisplayName<Role>)
            .ToList();

        return new IdentityUser(id, roles);
    }

    private List<string> GetClaimValues(string claimType) =>
        _httpContext.User.Claims
            .Where(claim => claim.Type == claimType)
            .Select(claim => claim.Value)
            .ToList();

    private string GetSingleClaimValue(string claimType) =>
        _httpContext.User.Claims
            .Single(claim => claim.Type == claimType)
            .Value;
}
