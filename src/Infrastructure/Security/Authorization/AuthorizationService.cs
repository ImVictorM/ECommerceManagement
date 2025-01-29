using Application.Common.Security.Authorization;
using Application.Common.Security.Authorization.Policies;
using Application.Common.Security.Identity;

using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Security.Authorization;

/// <summary>
/// Represents an authorization service implementation.
/// </summary>
public class AuthorizationService : IAuthorizationService
{
    private readonly IIdentityProvider _identityProvider;
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initiates a new instance of the <see cref="AuthorizationService"/> class.
    /// </summary>
    /// <param name="identityProvider">The identity provider.</param>
    /// <param name="serviceProvider">The service provider.</param>
    public AuthorizationService(IIdentityProvider identityProvider, IServiceProvider serviceProvider)
    {
        _identityProvider = identityProvider;
        _serviceProvider = serviceProvider;
    }

    /// <inheritdoc/>
    public async Task<bool> IsCurrentUserAuthorizedAsync<TRequest>(TRequest request, AuthorizationMetadata metadata)
    {
        if (!metadata.Roles.Any() && !metadata.Policies.Any())
        {
            return true;
        }

        var currentUser = _identityProvider.GetCurrentUserIdentity();

        // Check roles
        if (metadata.Roles.Except(currentUser.Roles).Any())
        {
            return false;
        }

        // Check policies
        foreach (var policyType in metadata.Policies)
        {
            var policy = (IPolicy<TRequest>)_serviceProvider.GetRequiredService(policyType);

            if (!await policy.IsAuthorizedAsync(request, currentUser))
            {
                return false;
            }
        }

        return true;
    }
}
