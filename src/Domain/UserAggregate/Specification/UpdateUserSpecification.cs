using SharedKernel.Models;

namespace Domain.UserAggregate.Specification;

/// <summary>
/// Specification to update an user.
/// </summary>
public class UpdateUserSpecification : CompositeSpecification<User>
{
    private readonly User _currentUser;

    /// <summary>
    /// Initiates a new instance of the <see cref="UpdateUserSpecification"/> class.
    /// </summary>
    /// <param name="currentUser">The current user.</param>
    public UpdateUserSpecification(User currentUser)
    {
        _currentUser = currentUser;
    }

    /// <inheritdoc/>
    public override bool IsSatisfiedBy(User userToUpdate)
    {
        return _currentUser.Id == userToUpdate.Id || (_currentUser.IsAdmin() && !userToUpdate.IsAdmin());
    }
}
