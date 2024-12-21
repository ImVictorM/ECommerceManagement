using Domain.UserAggregate.ValueObjects;
using SharedKernel.Authorization;
using SharedKernel.Interfaces;
using SharedKernel.Models;
using SharedKernel.ValueObjects;

namespace Domain.UserAggregate;

/// <summary>
/// Represents an user.
/// </summary>
public sealed class User : AggregateRoot<UserId>, IActivatable
{
    private readonly HashSet<UserRole> _userRoles = [];
    private readonly HashSet<Address> _userAddresses = [];

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
        Role role,
        string? phone
    )
    {
        Name = name;
        Email = email;
        Phone = phone;
        PasswordHash = passwordHash;

        IsActive = true;

        AssignRole(role);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="User"/> class.
    /// </summary>
    /// <param name="name">The user name.</param>
    /// <param name="email">The user email.</param>
    /// <param name="phone">The user phone (optional).</param>
    /// <param name="role">The user initial role to be associated.</param>
    /// <param name="passwordHash">The user password hash.</param>
    /// <param name="passwordSalt">The user password salt.</param>
    public static User Create(
        string name,
        Email email,
        string passwordHash,
        string passwordSalt,
        Role? role = null,
        string? phone = null
    )
    {
        var ph = PasswordHash.Create(passwordHash, passwordSalt);

        var user = new User(
            name,
            email,
            ph,
            role ?? Role.Customer,
            phone
        );

        return user;
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
    /// Relate the user with a role.
    /// </summary>
    /// <param name="role">The role to be assigned to the user.</param>
    public void AssignRole(Role role)
    {
        _userRoles.Add(UserRole.Create(role.Id));
    }

    /// <summary>
    /// List the name of the user roles.
    /// </summary>
    /// <returns>A list of names containing the user roles.</returns>
    public IEnumerable<string> GetRoleNames()
    {
        var userRoleIds = UserRoles.Select(ur => ur.RoleId).ToList();

        return Role.List(role => userRoleIds.Contains(role.Id)).Select(role => role.Name);
    }

    /// <inheritdoc/>
    public void Deactivate()
    {
        IsActive = false;
    }

    /// <summary>
    /// Checks if the user has the admin role assigned to them.
    /// </summary>
    /// <returns>A boolean value indicating if the user is an administrator.</returns>
    public bool IsAdmin()
    {
        return Role.HasAdminRole(UserRoles.Select(ur => ur.RoleId));
    }

    /// <summary>
    /// Adds addresses to the user.
    /// </summary>
    /// <param name="addresses">The addresses to be added.</param>
    public void AssignAddress(params Address[] addresses)
    {
        foreach (var address in addresses)
        {
            _userAddresses.Add(address);
        }
    }
}
