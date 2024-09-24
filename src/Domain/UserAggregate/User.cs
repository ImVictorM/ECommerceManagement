using Domain.Common.Models;
using Domain.UserAggregate.Entities;
using Domain.UserAggregate.ValueObjects;

namespace Domain.UserAggregate;

/// <summary>
/// Represents an user.
/// </summary>
public sealed class User : AggregateRoot<UserId>
{
    /// <summary>
    /// The user roles.
    /// </summary>
    private readonly List<UserRole> _roles = [];
    /// <summary>
    /// The user addresses.
    /// </summary>
    private readonly List<UserAddress>? _addresses = [];

    /// <summary>
    /// Gets the user name.
    /// </summary>
    public string Name { get; private set; } = string.Empty;
    /// <summary>
    /// Gets the user email.
    /// </summary>
    public string Email { get; private set; } = string.Empty;
    /// <summary>
    /// Gets the user phone.
    /// </summary>
    public string? Phone { get; private set; }
    /// <summary>
    /// Gets the user password hash.
    /// </summary>
    public string PasswordHash { get; private set; } = string.Empty;
    /// <summary>
    /// Gets a boolean value indicating if the user is active in the system.
    /// </summary>
    public bool IsActive { get; private set; }
    /// <summary>
    /// Gets the user roles.
    /// </summary>
    public IReadOnlyList<UserRole> Roles => _roles.AsReadOnly();
    /// <summary>
    /// Gets the user related addresses.
    /// </summary>
    public IReadOnlyList<UserAddress>? Addresses => _addresses?.AsReadOnly();

    /// <summary>
    /// Initializes a new instance of the <see cref="User"/> class.
    /// </summary>
    private User() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="User"/> class with default customer role and status of activated.
    /// </summary>
    /// <param name="name">The user name.</param>
    /// <param name="email">The user email.</param>
    /// <param name="phone">The user phone (optional).</param>
    /// <param name="passwordHash">The user password hashed.</param>
    private User(
        string name,
        string email,
        string? phone,
        string passwordHash
    ) : base(UserId.Create())
    {
        Name = name;
        Email = email;
        Phone = phone;
        PasswordHash = passwordHash;
        IsActive = true;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="User"/> class.
    /// </summary>
    /// <param name="name">The user name.</param>
    /// <param name="email">The user email.</param>
    /// <param name="phone">The user phone (optional).</param>
    /// <param name="passwordHash">The user password hashed.</param>
    public static User Create(
        string name,
        string email,
        string? phone,
        string passwordHash
    )
    {
        return new User(name, email, phone, passwordHash);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="User"/> class.
    /// </summary>
    /// <param name="name">The user name.</param>
    /// <param name="email">The user email.</param>
    /// <param name="passwordHash">The user password hashed.</param>
    public static User Create(
        string name,
        string email,
        string passwordHash
    )
    {
        return new User(name, email, null, passwordHash);
    }
}
