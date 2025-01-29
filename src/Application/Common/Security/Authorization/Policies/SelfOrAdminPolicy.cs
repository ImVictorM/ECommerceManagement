using Application.Common.Security.Authorization.Requests;
using Application.Common.Security.Identity;

namespace Application.Common.Security.Authorization.Policies;

/// <summary>
/// Defines a policy that authorizes the user if the current user is the same
/// as the request user or if the current user is an administrator.
/// </summary>
public sealed class SelfOrAdminPolicy<TRequest>
    : IPolicy<TRequest> where TRequest : IUserSpecificResource
{
    /// <inheritdoc/>
    public Task<bool> IsAuthorizedAsync(TRequest request, IdentityUser currentUser)
    {
        return Task.FromResult(request.UserId == currentUser.Id || currentUser.IsAdmin());
    }
}
