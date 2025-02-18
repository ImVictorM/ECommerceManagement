using Application.Common.Persistence;
using Application.Common.Security.Authorization.Requests;
using Application.Common.Security.Identity;

using Domain.UserAggregate.ValueObjects;

namespace Application.Common.Security.Authorization.Policies;

/// <summary>
/// Represents a policy to update a user.
/// Users can update themselves. Admins can update other non-admin users.
/// </summary>
public sealed class RestrictedUpdatePolicy<TRequest> : IPolicy<TRequest>
    where TRequest : IUserSpecificResource
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Creates a new instance of the <see cref="RestrictedUpdatePolicy{T}"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    public RestrictedUpdatePolicy(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc/>
    public async Task<bool> IsAuthorizedAsync(TRequest request, IdentityUser currentUser)
    {
        var userToBeUpdatedId = request.UserId;
        var currentUserIsAdmin = currentUser.IsAdmin();

        var userToBeUpdated = await _unitOfWork.UserRepository.FindByIdAsync(UserId.Create(userToBeUpdatedId));

        if (userToBeUpdated == null)
        {
            return false;
        }

        var userToBeUpdatedIsAdmin = userToBeUpdated.IsAdmin();

        return currentUser.Id == userToBeUpdatedId || (currentUserIsAdmin && !userToBeUpdatedIsAdmin);
    }
}
