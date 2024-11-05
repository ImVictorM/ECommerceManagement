using System.Globalization;
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
    /// <param name="id">The user id.</param>
    /// <param name="name">The user name.</param>
    /// <param name="email">The user email.</param>
    /// <param name="phone">The user phone.</param>
    /// <returns>A new instance of the <see cref="UpdateUserCommand"/> class.</returns>
    public static UpdateUserCommand CreateCommand(
        string? id = null,
        string? name = null,
        string? email = null,
        string? phone = null
    )
    {
        return new UpdateUserCommand(
            id ?? DomainConstants.User.Id.ToString(CultureInfo.InvariantCulture),
            name ?? DomainConstants.User.Name,
            phone ?? DomainConstants.User.Phone,
            email ?? DomainConstants.Email.Value
        );
    }
}
