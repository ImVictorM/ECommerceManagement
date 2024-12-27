using Application.Users.Commands.UpdateUser;
using Domain.UnitTests.TestUtils.Constants;

namespace Application.UnitTests.Users.Commands.TestUtils;

/// <summary>
/// Utilities for the <see cref="UpdateUserCommand"/> command.
/// </summary>
public static class UpdateUserCommandUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="UpdateUserCommand"/> class.
    /// </summary>
    /// <param name="idCurrentUser">The id of the user responsible for the update.</param>
    /// <param name="idUserToUpdate">The user to be updated id.</param>
    /// <param name="name">The user name.</param>
    /// <param name="email">The user email.</param>
    /// <param name="phone">The user phone.</param>
    /// <returns>A new instance of the <see cref="UpdateUserCommand"/> class.</returns>
    public static UpdateUserCommand CreateCommand(
        string? idCurrentUser = null,
        string? idUserToUpdate = null,
        string? name = null,
        string? email = null,
        string? phone = null
    )
    {
        return new UpdateUserCommand(
            idCurrentUser ?? DomainConstants.User.Id.ToString(),
            idUserToUpdate ?? DomainConstants.User.Id.ToString(),
            name ?? DomainConstants.User.Name,
            phone ?? DomainConstants.User.Phone,
            email ?? DomainConstants.User.Email
        );
    }
}
