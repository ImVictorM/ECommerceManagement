using Application.Common.Security.Identity;

namespace Application.Common.Security.Authorization.Policies;

/// <summary>
/// Represents a contract for defining and evaluating specific authorization
/// policies.
/// </summary>
public interface IPolicy<TRequest>
{
    /// <summary>
    /// Evaluates whether the specified request is authorized for the given
    /// user.
    /// </summary>
    /// <param name="request">
    /// The request to evaluate.
    /// </param>
    /// <param name="currentUser">
    /// An instance of <see cref="IdentityUser"/> representing the user for
    /// whom authorization is being evaluated.
    /// </param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A boolean value indicating if the current user is authorized to perform
    /// the request.
    /// </returns>
    Task<bool> IsAuthorizedAsync(
        TRequest request,
        IdentityUser currentUser,
        CancellationToken cancellationToken = default
    );
}
