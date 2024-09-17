using Domain.Common.Models;
using Domain.Users.Entities;
using Domain.Users.ValueObjects;

namespace Domain.Users;

/// <summary>
/// Represents an user.
/// </summary>
public sealed class User : AggregateRoot<UserId>
{
    /// <summary>
    /// Gets the user name.
    /// </summary>
    public string Name { get; }
    /// <summary>
    /// Gets the user email.
    /// </summary>
    public string Email { get; }
    /// <summary>
    /// Gets the user phone.
    /// </summary>
    public string? Phone { get; }
    /// <summary>
    /// Gets the user password hash.
    /// </summary>
    public string PasswordHash { get; }
    /// <summary>
    /// Gets a boolean value indicating if the user is active in the system.
    /// </summary>
    public bool IsActive { get; }
    /// <summary>
    /// Gets the user roles.
    /// </summary>
    public IEnumerable<Role> Roles { get; }

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
    ): base(UserId.Create())
    {
        Name = name;
        Email = email;
        Phone = phone;
        PasswordHash = passwordHash;
        IsActive = true;
        Roles = [Role.Create("customer")];
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
