using Domain.RoleAggregate;
using Domain.RoleAggregate.Enums;
using Domain.RoleAggregate.ValueObjects;

namespace Domain.UnitTests.TestUtils;

/// <summary>
/// Role utilities.
/// </summary>
public static class RoleUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="RoleId"/> class.
    /// </summary>
    /// <param name="id">The identifier value.</param>
    /// <returns>A new instance of the <see cref="RoleId"/> class.</returns>
    public static RoleId CreateRoleId(long? id = null)
    {
        return RoleId.Create(id ?? Constants.TestConstants.Role.Id);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Role"/> class.
    /// </summary>
    /// <param name="type">The role type.</param>
    /// <returns>a new instance of the <see cref="Role"/> class.</returns>
    public static Role CreateRole(RoleTypes type)
    {
        return Role.Create(type);
    }
}
