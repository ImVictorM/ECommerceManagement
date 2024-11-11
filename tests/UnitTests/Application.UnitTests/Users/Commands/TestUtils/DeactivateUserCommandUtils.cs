using System.Globalization;
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
    /// <param name="id">The user id.</param>
    /// <returns>A new instance of the <see cref="DeactivateUserCommand"/> class.</returns>
    public static DeactivateUserCommand CreateCommand(string? id = null)
    {
        return new DeactivateUserCommand(id ?? DomainConstants.User.Id.ToString(CultureInfo.InvariantCulture));
    }
}
