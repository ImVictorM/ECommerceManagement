using Application.Users.Commands.DeactivateUser;
using Domain.UnitTests.TestUtils.Constants;

namespace Application.UnitTests.Users.Commands.TestUtils;

/// <summary>
/// Utilities for the <see cref="DeactivateUserCommand"/> command.
/// </summary>
public static class DeactivateUserCommandUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="DeactivateUserCommand"/> class.
    /// </summary>
    /// <param name="idCurrentUser">The current user id.</param>
    /// <param name="idUserToDeactivate">The user id.</param>
    /// <returns>A new instance of the <see cref="DeactivateUserCommand"/> class.</returns>
    public static DeactivateUserCommand CreateCommand(
        string? idCurrentUser = null,
        string? idUserToDeactivate = null
    )
    {
        return new DeactivateUserCommand(
            idCurrentUser ?? DomainConstants.User.Id.ToString(),
            idUserToDeactivate ?? DomainConstants.User.Id.ToString()
        );
    }
}
