using Application.Common.Persistence.Repositories;
using Application.Common.Security.Authorization.Requests;
using Application.Common.Security.Identity;

using Domain.UserAggregate.ValueObjects;

namespace Application.Common.Security.Authorization.Policies;

internal sealed class RestrictedUpdatePolicy<TRequest> : IPolicy<TRequest>
    where TRequest : IUserSpecificResource
{
    private readonly IUserRepository _userRepository;

    public RestrictedUpdatePolicy(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    /// <inheritdoc/>
    public async Task<bool> IsAuthorizedAsync(TRequest request, IdentityUser currentUser)
    {
        var userToBeUpdatedId = request.UserId;
        var currentUserIsAdmin = currentUser.IsAdmin();

        var userToBeUpdated = await _userRepository.FindByIdAsync(UserId.Create(userToBeUpdatedId));

        if (userToBeUpdated == null)
        {
            return false;
        }

        var userToBeUpdatedIsAdmin = userToBeUpdated.IsAdmin();

        return currentUser.Id == userToBeUpdatedId || (currentUserIsAdmin && !userToBeUpdatedIsAdmin);
    }
}
