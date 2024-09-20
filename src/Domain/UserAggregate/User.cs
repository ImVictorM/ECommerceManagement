using Domain.Common.Models;
using Domain.OrderAggregate.ValueObjects;
using Domain.ProductFeedbackAggregate.ValueObjects;
using Domain.UserAggregate.Entities;
using Domain.UserAggregate.ValueObjects;
using Domain.UserRoleAggregate.ValueObjects;

namespace Domain.UserAggregate;

/// <summary>
/// Represents an user.
/// </summary>
public sealed class User : AggregateRoot<UserId>
{
    /// <summary>
    /// Gets the user name.
    /// </summary>
    public string Name { get; private set; }
    /// <summary>
    /// Gets the user email.
    /// </summary>
    public string Email { get; private set; }
    /// <summary>
    /// Gets the user phone.
    /// </summary>
    public string? Phone { get; private set; }
    /// <summary>
    /// Gets the user password hash.
    /// </summary>
    public string PasswordHash { get; private set; }
    /// <summary>
    /// Gets a boolean value indicating if the user is active in the system.
    /// </summary>
    public bool IsActive { get; private set; }
    /// <summary>
    /// Gets the user roles.
    /// </summary>
    public UserRoleId UserRoleId { get; private set; }
    /// <summary>
    /// Gets the user related addresses.
    /// </summary>
    public UserAddress? UserAddress { get; private set; }
    /// <summary>
    /// Gets the user order ids.
    /// </summary>
    public IEnumerable<OrderId>? OrderIds { get; private set; }
    /// <summary>
    /// Gets the user feedback ids on products.
    /// </summary>
    public IEnumerable<ProductFeedbackId>? ProductFeedbackIds { get; private set; }


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
        string passwordHash,
        UserRoleId userRoleId
    ) : base(UserId.Create())
    {
        Name = name;
        Email = email;
        Phone = phone;
        PasswordHash = passwordHash;
        UserRoleId = userRoleId;
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
        string passwordHash,
        UserRoleId userRoleId
    )
    {
        return new User(name, email, phone, passwordHash, userRoleId);
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
        string passwordHash,
        UserRoleId userRoleId
    )
    {
        return new User(name, email, null, passwordHash, userRoleId);
    }
}
