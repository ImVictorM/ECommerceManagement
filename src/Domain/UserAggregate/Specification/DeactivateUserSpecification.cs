using SharedKernel.Models;

namespace Domain.UserAggregate.Specification;

/// <summary>
/// Specification to deactivate an user.
/// </summary>
public class DeactivateUserSpecification : CompositeSpecification<User>
{
    private readonly User _currentUser;

    /// <summary>
    /// Initiates a new instance of the <see cref="DeactivateUserSpecification"/> class.
    /// </summary>
    /// <param name="currentUser">The current user.</param>
    public DeactivateUserSpecification(User currentUser)
    {
        _currentUser = currentUser;
    }

    /// <inheritdoc/>
    public override bool IsSatisfiedBy(User userToDeactivate)
    {
        if (_currentUser.Id != userToDeactivate.Id)
        {
            // Only admins are allowed to update other users that are not admins
            return _currentUser.IsAdmin() && !userToDeactivate.IsAdmin();
        }
        else
        {
            // Admins are not allowed to deactivate themselves
            return !_currentUser.IsAdmin();
        }
    }
}
