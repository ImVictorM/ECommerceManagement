using Domain.Common.Errors;
using Domain.Common.Models;

namespace Domain.UserAggregate.ValueObjects;

/// <summary>
/// Represents a role.
/// </summary>
public sealed class Role : ValueObject
{
    /// <summary>
    /// Represents the customer role.
    /// </summary>
    public static readonly Role Customer = new(nameof(Customer).ToLowerInvariant());
    /// <summary>
    /// Represents the administrator role.
    /// </summary>
    public static readonly Role Admin = new(nameof(Admin).ToLowerInvariant());

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
    private Role(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Role"/> class.
    /// </summary>
    /// <param name="name">The name of the role.</param>
    public static Role Create(string name)
    {
        if (GetRoleByName(name) == null) throw new DomainValidationException($"The {name} role does not exist");

        return new Role(name);
    }

    /// <summary>
    /// Gets a role by name, or null if not found.
    /// </summary>
    /// <param name="name">The role name.</param>
    /// <returns>The role or null.</returns>
    private static Role? GetRoleByName(string name)
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

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Name;
    }
}
