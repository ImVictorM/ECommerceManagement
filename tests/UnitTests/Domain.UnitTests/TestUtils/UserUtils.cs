using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;

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
    /// <param name="roleId">The user role id.</param>
    /// <param name="email">The user email.</param>
    /// <returns>A new instance of the <see cref="User"/> class.</returns>
    public static User CreateUser(
        string? name = null,
        string? passwordHash = null,
        string? passwordSalt = null,
        string? phone = null,
        RoleId? roleId = null,
        string? email = null
    )
    {
        return User.Create(
            name ?? Constants.DomainConstants.User.Name,
            EmailUtils.CreateEmail(email: email),
            passwordHash ?? Constants.DomainConstants.User.PasswordHash,
            passwordSalt ?? Constants.DomainConstants.User.PasswordSalt,
            roleId ?? Constants.DomainConstants.User.Role.Id,
            phone ?? Constants.DomainConstants.User.Phone
        );
    }

    /// <summary>
    /// Creates a list of users with unique name and email.
    /// </summary>
    /// <param name="count">The quantity of users to be created.</param>
    /// <param name="active">Specify if the created users should be active or inactive.</param>
    /// <returns>A list of unique users.</returns>
    public static IEnumerable<User> CreateUsers(int count = 1, bool? active = true)
    {
        var users = Enumerable
            .Range(0, count)
            .Select(index =>
                CreateUser(
                    name: Constants.DomainConstants.User.UserNameFromIndex(index),
                    email: Constants.DomainConstants.Email.EmailFromIndex(index)
                )
            );

        if (active == null)
        {
            return users;
        }

        foreach (var user in users)
        {
            user.MakeInactive();
        }

        return users;
    }
}
