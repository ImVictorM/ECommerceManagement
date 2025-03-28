using Domain.UserAggregate.ValueObjects;

using SharedKernel.Errors;
using SharedKernel.Interfaces;
using SharedKernel.Models;
using SharedKernel.ValueObjects;

namespace Domain.UserAggregate;

/// <summary>
/// Represents a user.
/// </summary>
public sealed class User : AggregateRoot<UserId>, IActivatable
{
    private readonly HashSet<UserRole> _userRoles = [];
    private readonly HashSet<Address> _userAddresses = [];

    /// <summary>
    /// Gets the user name.
    /// </summary>
    public string Name { get; private set; } = null!;
    /// <summary>
    /// Gets the user email.
    /// </summary>
    public Email Email { get; private set; } = null!;
    /// <summary>
    /// Gets the user phone.
    /// </summary>
    public string? Phone { get; private set; }
    /// <summary>
    /// Gets the user password hash.
    /// </summary>
    public PasswordHash PasswordHash { get; private set; } = null!;
    /// <inheritdoc/>
    public bool IsActive { get; private set; }
    /// <summary>
    /// Gets the user roles.
    /// </summary>
    public IReadOnlySet<UserRole> UserRoles => _userRoles;
    /// <summary>
    /// Gets the user related addresses.
    /// </summary>
    public IReadOnlySet<Address> UserAddresses => _userAddresses;

    private User() { }

    private User(
        string name,
        Email email,
        PasswordHash passwordHash,
        IReadOnlySet<UserRole> roles,
        string? phone
    )
    {
        Name = name;
        Email = email;
        Phone = phone;
        PasswordHash = passwordHash;

        IsActive = true;

        if (roles.Count == 0)
        {
            throw new EmptyArgumentException("Users must have at least one role");
        }

        _userRoles.UnionWith(roles);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="User"/> class,
    /// Creating a new customer user.
    /// </summary>
    /// <param name="name">The user name.</param>
    /// <param name="email">The user email.</param>
    /// <param name="phone">The user phone (optional).</param>
    /// <param name="passwordHash">The user password hash.</param>
    public static User CreateCustomer(
        string name,
        Email email,
        PasswordHash passwordHash,
        string? phone = null
    )
    {

        return new User(
            name,
            email,
            passwordHash,
            new HashSet<UserRole>()
            {
                UserRole.Create(Role.Customer)
            },
            phone
        );
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="User"/> class,
    /// Creating a new admin user.
    /// </summary>
    /// <param name="name">The user name.</param>
    /// <param name="email">The user email.</param>
    /// <param name="phone">The user phone (optional).</param>
    /// <param name="passwordHash">The user password hash.</param>
    public static User CreateAdmin(
        string name,
        Email email,
        PasswordHash passwordHash,
        string? phone = null
     )
    {
        return new User(
            name,
            email,
            passwordHash,
            new HashSet<UserRole>()
            {
                UserRole.Create(Role.Admin)
            },
            phone
        );
    }

    /// <summary>
    /// Updates the user data.
    /// </summary>
    /// <param name="name">The new user name.</param>
    /// <param name="phone">The new user phone.</param>
    /// <param name="email">The new user email.</param>
    public void Update(
        string? name = null,
        string? phone = null,
        Email? email = null)
    {
        Name = name ?? Name;
        Phone = phone ?? Phone;
        Email = email ?? Email;
    }

    /// <summary>
    /// Verifies if the current user is an administrator.
    /// </summary>
    /// <returns>
    /// A bool value indicating if the user is an administrator.
    /// </returns>
    public bool IsAdmin()
    {
        return UserRoles.Any(ur => ur.Role == Role.Admin);
    }

    /// <inheritdoc/>
    public void Deactivate()
    {
        IsActive = false;
    }

    /// <summary>
    /// Assign addresses to the user.
    /// </summary>
    /// <param name="addresses">The addresses to be added.</param>
    public void AssignAddress(params Address[] addresses)
    {
        _userAddresses.UnionWith(addresses);
    }
}
