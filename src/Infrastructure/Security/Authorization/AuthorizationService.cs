using Application.Common.Security.Authorization;
using Application.Common.Security.Authorization.Policies;
using Application.Common.Security.Identity;

using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Security.Authorization;

internal sealed class AuthorizationService : IAuthorizationService
{
    private readonly IIdentityProvider _identityProvider;
    private readonly IServiceProvider _serviceProvider;

    public AuthorizationService(
        IIdentityProvider identityProvider,
        IServiceProvider serviceProvider
    )
    {
        _identityProvider = identityProvider;
        _serviceProvider = serviceProvider;
    }

    public async Task<bool> IsCurrentUserAuthorizedAsync<TRequest>(
        TRequest request,
        AuthorizationMetadata metadata,
        CancellationToken cancellationToken = default
    )
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
            var policy = (IPolicy<TRequest>)_serviceProvider
                .GetRequiredService(policyType);

            var isAuthorized = await policy.IsAuthorizedAsync(
                request,
                currentUser,
                cancellationToken
            );

            if (!isAuthorized)
            {
                return false;
            }
        }

        return true;
    }
}
