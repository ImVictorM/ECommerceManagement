using Application.Common.Security.Authorization;
using Application.Common.Security.Authorization.Policies;
using Application.Common.Security.Authorization.Requests;
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

    internal AuthorizationService(IIdentityProvider identityProvider, IServiceProvider serviceProvider)
    {
        _identityProvider = identityProvider;
        _serviceProvider = serviceProvider;
    }

    /// <inheritdoc/>
    public async Task<bool> IsCurrentUserAuthorizedAsync<T>(
        RequestWithAuthorization<T> request,
        IReadOnlyList<string> requiredRoleNames,
        IReadOnlyList<Type> requiredPolicyTypes
    )
    {
        var currentUser = _identityProvider.GetCurrentUserIdentity();

        if (requiredRoleNames.Except(currentUser.Roles).Any())
        {
            return false;
        }

        foreach (var policyType in requiredPolicyTypes)
        {
            var policy = (IPolicy)_serviceProvider.GetRequiredService(policyType);

            if (!await policy.IsAuthorizedAsync(request, currentUser))
            {
                return false;
            }
        }

        return true;
    }
}
