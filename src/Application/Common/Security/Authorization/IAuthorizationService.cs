using Application.Common.Security.Authorization.Requests;

namespace Application.Common.Security.Authorization;

/// <summary>
/// Service to handle authorization.
/// </summary>
public interface IAuthorizationService
{
    /// <summary>
    /// Verifies if the current user is authorized.
    /// </summary>
    /// <typeparam name="T">The request return type.</typeparam>
    /// <param name="request">The current request.</param>
    /// <param name="requiredRoleNames">The authorization required roles.</param>
    /// <param name="requiredPolicyTypes">The authorization required policy types.</param>
    /// <returns>A bool value indicating if the user is authorized.</returns>
    Task<bool> IsCurrentUserAuthorizedAsync<T>(
        IRequestWithAuthorization<T> request,
        IReadOnlyList<string> requiredRoleNames,
        IReadOnlyList<Type> requiredPolicyTypes
    );
}
