using Application.Common.Persistence.Repositories;
using Application.Common.Security.Authorization.Requests;
using Application.Common.Security.Identity;

using Domain.UserAggregate.ValueObjects;

namespace Application.Common.Security.Authorization.Policies;

internal sealed class RestrictedDeactivationPolicy<TRequest> :
    IPolicy<TRequest> where TRequest : IUserSpecificResource
{
    private readonly IUserRepository _userRepository;

    public RestrictedDeactivationPolicy(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    /// <inheritdoc/>
    public async Task<bool> IsAuthorizedAsync(TRequest request, IdentityUser currentUser)
    {
        var currentUserId = UserId.Create(currentUser.Id);
        var userToBeDeactivatedId = UserId.Create(request.UserId);
        var currentUserIsAdmin = currentUser.IsAdmin();

        if (currentUserId != userToBeDeactivatedId)
        {
            var userToBeDeactivated = await _userRepository.FindByIdAsync(userToBeDeactivatedId);

            if (userToBeDeactivated == null)
            {
                return false;
            }

            var userToBeDeactivatesIsAdmin = userToBeDeactivated.IsAdmin();

            // Only admins are allowed to deactivate other users that are not admins
            return currentUserIsAdmin && !userToBeDeactivatesIsAdmin;
        }
        else
        {
            // Admins are not allowed to deactivate themselves
            return !currentUserIsAdmin;
        }
    }
}
