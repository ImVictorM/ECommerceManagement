using Application.Common.Extensions.Users;
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
public sealed class RestrictedDeactivationPolicy : IPolicy
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="RestrictedDeactivationPolicy"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    public RestrictedDeactivationPolicy(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc/>
    public async Task<bool> IsAuthorizedAsync<T>(RequestWithAuthorization<T> request, IdentityUser currentUser)
    {
        if (request.UserId == null)
        {
            throw new ArgumentException($"The user id is required for the authorization process. Policy: {nameof(RestrictedDeactivationPolicy)}");
        }

        var currentUserId = UserId.Create(currentUser.Id);
        var userToBeDeactivatedId = UserId.Create(request.UserId);
        var currentUserIsAdmin = currentUser.IsAdmin();

        if (currentUserId != userToBeDeactivatedId)
        {
            var userToBeDeactivated = await _unitOfWork.UserRepository.FindByIdAsync(userToBeDeactivatedId);

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
