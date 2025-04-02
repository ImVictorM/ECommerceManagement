using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;

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
    /// Creates a new customer.
    /// </summary>
    /// <param name="id">The user identifier.</param>
    /// <param name="name">The user name.</param>
    /// <param name="passwordHash">The user password hash.</param>
    /// <param name="phone">The user phone.</param>
    /// <param name="email">The user email.</param>
    /// <param name="addresses">The user addresses.</param>
    /// <param name="active">A boolean value indicating the user state.</param>
    /// <returns>A new instance of the <see cref="User"/> class.</returns>
    public static User CreateCustomer(
        UserId? id = null,
        string? name = null,
        Email? email = null,
        PasswordHash? passwordHash = null,
        string? phone = null,
        IReadOnlySet<Address>? addresses = null,
        bool active = true
    )
    {
        var user = User.CreateCustomer(
            name ?? CreateUserName(),
            email ?? EmailUtils.CreateEmail(),
            passwordHash ?? PasswordHashUtils.Create(),
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
    /// Creates a new instance of the <see cref="User"/> class.
    /// Creates a new administrator.
    /// </summary>
    /// <param name="id">The user identifier.</param>
    /// <param name="name">The user name.</param>
    /// <param name="passwordHash">The user password hash.</param>
    /// <param name="phone">The user phone.</param>
    /// <param name="email">The user email.</param>
    /// <param name="addresses">The user addresses.</param>
    /// <param name="active">A boolean value indicating the user state.</param>
    /// <returns>A new instance of the <see cref="User"/> class.</returns>
    public static User CreateAdmin(
        UserId? id = null,
        string? name = null,
        Email? email = null,
        PasswordHash? passwordHash = null,
        string? phone = null,
        IReadOnlySet<Address>? addresses = null,
        bool active = true
    )
    {
        var user = User.CreateAdmin(
           name ?? CreateUserName(),
           email ?? EmailUtils.CreateEmail(),
           passwordHash ?? PasswordHashUtils.Create(),
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
    /// Generates a new user name.
    /// </summary>
    /// <returns>The new user name.</returns>
    public static string CreateUserName()
    {
        return _faker.Name.FullName();
    }

    /// <summary>
    /// Creates a list of customer users with unique name and email.
    /// </summary>
    /// <param name="count">The quantity of users to be created.</param>
    /// <param name="active">
    /// Specify if the created users should be active or inactive.
    /// </param>
    /// <returns>A list of unique customer users.</returns>
    public static IReadOnlyList<User> CreateCustomers(
        int count = 1,
        bool active = true
    )
    {
        var users = Enumerable
            .Range(0, count)
            .Select(index =>
                CreateCustomer(
                    id: UserId.Create(index + 1),
                    active: active
                )
            )
            .ToList();

        return users;
    }
}
