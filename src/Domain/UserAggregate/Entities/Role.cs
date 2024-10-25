using Domain.Common.Errors;
using Domain.Common.Models;
using Domain.UserAggregate.ValueObjects;
using SharedResources.Extensions;

namespace Domain.UserAggregate.Entities;

/// <summary>
/// Represents a role.
/// </summary>
public sealed class Role : Entity<RoleId>
{
    /// <summary>
    /// Represents the administrator role.
    /// </summary>
    public static readonly Role Admin = new(RoleId.Create(1), nameof(Admin).ToLowerSnakeCase());

    /// <summary>
    /// Represents the customer role.
    /// </summary>
    public static readonly Role Customer = new(RoleId.Create(2), nameof(Customer).ToLowerSnakeCase());

    /// <summary>
    /// Gets the role name.
    /// </summary>
    public string Name { get; } = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="Role"/> class.
    /// </summary>
    private Role() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Role"/> class.
    /// </summary>
    /// <param name="name">The name of the role.</param>
    /// <param name="id">The role identifier.</param>
    private Role(RoleId id, string name) : base(id)
    {

        Name = name;

        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Role"/> class.
    /// </summary>
    /// <param name="name">The name of the role.</param>
    public static Role Create(string name)
    {
        return GetRoleByName(name) ?? throw new DomainValidationException($"The {name} role does not exist");
    }

    /// <summary>
    /// Gets a role by name, or null if not found.
    /// </summary>
    /// <param name="name">The role name.</param>
    /// <returns>The role or null.</returns>
    public static Role? GetRoleByName(string name)
    {
        return List().FirstOrDefault(r => r.Name == name);
    }

    /// <summary>
    /// Gets all the roles in a list format.
    /// </summary>
    /// <returns>All the roles.</returns>
    public static IEnumerable<Role> List()
    {
        return [Customer, Admin];
    }

    /// <summary>
    /// Gets the roles applying a filter condition.
    /// </summary>
    /// <param name="filter">The filter condition.</param>
    /// <returns>A list with roles that matches the filter condition.</returns>
    public static IEnumerable<Role> List(Func<Role, bool> filter)
    {
        return List().Where(filter);
    }
}
