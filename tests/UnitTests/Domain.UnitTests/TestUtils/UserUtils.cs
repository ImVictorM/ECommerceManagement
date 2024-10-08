using Domain.UserAggregate;

namespace Domain.UnitTests.TestUtils;

/// <summary>
/// User utilities.
/// </summary>
public static class UserUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="User"/> class.
    /// </summary>
    /// <param name="name">The user name.</param>
    /// <param name="passwordHash">The password hash.</param>
    /// <param name="passwordSalt">The password salt.</param>
    /// <param name="phone">The user phone.</param>
    /// <param name="email">The user email.</param>
    /// <returns>A new instance of the <see cref="User"/> class.</returns>
    public static User CreateUser(
        string? name = null,
        string? passwordHash = null,
        string? passwordSalt = null,
        string? phone = null,
        string? email = null
    )
    {
        return User.Create(
            name ?? Constants.TestConstants.User.Name,
            EmailUtils.CreateEmail(email: email),
            passwordHash ?? Constants.TestConstants.User.PasswordHash,
            passwordSalt ?? Constants.TestConstants.User.PasswordSalt,
            RoleUtils.CreateRoleId(),
            phone ?? Constants.TestConstants.User.Phone
        );
    }
}
