using Application.Common.Persistence;
using Application.Common.Security.Authorization.Requests;
using Application.Common.Security.Identity;

using Domain.UserAggregate.ValueObjects;

namespace Application.Common.Security.Authorization.Policies;

/// <summary>
/// Represents a policy to deactivate an user.
/// A non-admin user can deactivate themselves.
/// An admin can deactivate other non-admin users.
/// </summary>
public sealed class RestrictedDeactivationPolicy<TRequest> :
    IPolicy<TRequest> where TRequest : IUserSpecificResource
{
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Initiates a new instance of the <see cref="RestrictedDeactivationPolicy{TRequest}"/> class.
    /// </summary>
    /// <param name="userRepository">The user repository.</param>
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
