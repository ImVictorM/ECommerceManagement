using Application.Common.Security.Authorization.Requests;
using Application.Common.Security.Identity;

namespace Application.Common.Security.Authorization.Policies;

/// <summary>
/// Represents a contract for defining and evaluating authorization policies.
/// </summary>
public interface IPolicy
{
    /// <summary>
    /// Evaluates whether the specified request is authorized for the given user.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the result that the request produces.
    /// </typeparam>
    /// <param name="request">
    /// The request to evaluate, implementing <see cref="IRequestWithAuthorization{T}"/>.
    /// </param>
    /// <param name="currentUser">
    /// An instance of <see cref="IdentityUser"/> representing the user for whom authorization 
    /// is being evaluated.
    /// </param>
    /// <returns>
    /// A boolean value indicating if the user is authorized to perform the request.
    /// </returns>
    Task<bool> IsAuthorizedAsync<T>(
        IRequestWithAuthorization<T> request,
        IdentityUser currentUser
    );
}
