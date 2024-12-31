using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;

using SharedKernel.Authorization;
using SharedKernel.UnitTests.TestUtils;
using SharedKernel.UnitTests.TestUtils.Extensions;
using SharedKernel.ValueObjects;

using Bogus;

namespace Domain.UnitTests.TestUtils;

/// <summary>
/// Utilities for the <see cref="User"/> class.
/// </summary>
public static class UserUtils
{
    private static readonly Faker _faker = new();

    /// <summary>
    /// Creates a new instance of the <see cref="User"/> class.
    /// </summary>
    /// <param name="id">The user id.</param>
    /// <param name="name">The user name.</param>
    /// <param name="passwordHash">The user password hash.</param>
    /// <param name="phone">The user phone.</param>
    /// <param name="roles">The user roles.</param>
    /// <param name="email">The user email.</param>
    /// <param name="addresses">The user addresses.</param>
    /// <param name="active">Defines if the user should be inactive.</param>
    /// <returns>A new instance of the <see cref="User"/> class.</returns>
    public static User CreateUser(
        UserId? id = null,
        string? name = null,
        Email? email = null,
        PasswordHash? passwordHash = null,
        string? phone = null,
        IReadOnlySet<Role>? roles = null,
        IReadOnlySet<Address>? addresses = null,
        bool active = true
    )
    {
        var user = User.Create(
            name ?? _faker.Name.FullName(),
            email ?? EmailUtils.CreateEmail(),
            passwordHash ?? PasswordHashUtils.Create(),
            roles ?? new HashSet<Role>()
            {
                Role.Customer,
            },
            phone
        );

        if (id != null)
        {
            user.SetIdUsingReflection(id);
        }

        if (addresses != null)
        {
            user.AssignAddress([.. addresses]);
        }

        if (!active)
        {
            user.Deactivate();
        }

        return user;
    }

    /// <summary>
    /// Creates a list of users with unique name and email.
    /// </summary>
    /// <param name="count">The quantity of users to be created.</param>
    /// <param name="active">Specify if the created users should be active or inactive.</param>
    /// <returns>A list of unique users.</returns>
    public static IEnumerable<User> CreateUsers(int count = 1, bool active = true)
    {
        var users = Enumerable
            .Range(0, count)
            .Select(index =>
                CreateUser(
                    id: UserId.Create(index + 1),
                    active: active
                )
            );

        return users;
    }
}
