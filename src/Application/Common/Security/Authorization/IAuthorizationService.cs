namespace Application.Common.Security.Authorization;

/// <summary>
/// Service for handling authorization.
/// </summary>
public interface IAuthorizationService
{
    /// <summary>
    /// Checks if the current user is authorized to perform a request.
    /// </summary>
    /// <typeparam name="TRequest">The request type.</typeparam>
    /// <param name="request">The current request.</param>
    /// <param name="metadata">The authorization metadata for the request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A bool value indicating if the user is authorized.</returns>
    Task<bool> IsCurrentUserAuthorizedAsync<TRequest>(
        TRequest request,
        AuthorizationMetadata metadata,
        CancellationToken cancellationToken = default
    );
}
