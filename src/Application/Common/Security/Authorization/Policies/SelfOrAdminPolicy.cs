using Application.Common.Security.Authorization.Requests;
using Application.Common.Security.Identity;

namespace Application.Common.Security.Authorization.Policies;

internal sealed class SelfOrAdminPolicy<TRequest>
    : IPolicy<TRequest> where TRequest : IUserSpecificResource
{
    public Task<bool> IsAuthorizedAsync(
        TRequest request,
        IdentityUser currentUser,
        CancellationToken cancellationToken = default
    )
    {
        return Task.FromResult(
            request.UserId == currentUser.Id || currentUser.IsAdmin()
        );
    }
}
