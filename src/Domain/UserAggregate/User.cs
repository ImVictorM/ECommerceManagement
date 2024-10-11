using Domain.Common.Interfaces;
using Domain.Common.Models;
using Domain.Common.ValueObjects;
using Domain.UserAggregate.Entities;
using Domain.UserAggregate.ValueObjects;

namespace Domain.UserAggregate;

/// <summary>
/// Represents an user.
/// </summary>
public sealed class User : AggregateRoot<UserId>, ISoftDeletable
{
    /// <summary>
    /// The user roles.
    /// </summary>
    private readonly List<UserRole> _userRoles = [];
    /// <summary>
    /// The user addresses.
    /// </summary>
    private readonly List<UserAddress> _userAddresses = [];

    /// <summary>
    /// Gets the user name.
    /// </summary>
    public string Name { get; private set; } = string.Empty;
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
    /// <summary>
    /// Gets a boolean value indicating if the user is active in the system.
    /// </summary>
    public bool IsActive { get; private set; }
    /// <summary>
    /// Gets the user roles.
    /// </summary>
    public IReadOnlyList<UserRole> UserRoles => _userRoles.AsReadOnly();
    /// <summary>
    /// Gets the user related addresses.
    /// </summary>
    public IReadOnlyList<UserAddress> UserAddresses => _userAddresses.AsReadOnly();

    /// <summary>
    /// Initializes a new instance of the <see cref="User"/> class.
    /// </summary>
    private User() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="User"/> class with default customer role and status of activated.
    /// </summary>
    /// <param name="name">The user name.</param>
    /// <param name="email">The user email.</param>
    /// <param name="role">The user associated role.</param>
    /// <param name="passwordHash">The user password hashed.</param>
    /// <param name="phone">The user phone (optional).</param>
    private User(
        string name,
        Email email,
        PasswordHash passwordHash,
        Role role,
        string? phone
    )
    {
        Name = name;
        Email = email;
        Phone = phone;
        PasswordHash = passwordHash;
        IsActive = true;

        AddUserRole(role);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="User"/> class.
    /// </summary>
    /// <param name="name">The user name.</param>
    /// <param name="email">The user email.</param>
    /// <param name="phone">The user phone (optional).</param>
    /// <param name="role">The user associated role.</param>
    /// <param name="passwordHash">The user password hash.</param>
    /// <param name="passwordSalt">The user password salt.</param>
    public static User Create(
        string name,
        Email email,
        string passwordHash,
        string passwordSalt,
        Role role,
        string? phone = null
    )
    {
        var ph = PasswordHash.Create(passwordHash, passwordSalt);

        var user = new User(
            name,
            email,
            ph,
            role,
            phone
        );

        return user;
    }

    /// <summary>
    /// Relate the user with a role.
    /// </summary>
    /// <param name="role">The role to be related with the user.</param>
    public void AddUserRole(Role role)
    {
        if (UserRoles.Any(ur => ur.Role == role))
        {
            return;
        }

        _userRoles.Add(UserRole.Create(role));
    }

    /// <inheritdoc/>
    public void MakeInactive()
    {
        IsActive = false;
    }
}
