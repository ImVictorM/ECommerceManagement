using Application.Common.Interfaces.Persistence;
using Domain.RoleAggregate;

namespace Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository to interact and persist user roles.
/// </summary>
public class RoleRepository : IRoleRepository
{
    /// <inheritdoc/>
    public Task<IReadOnlyList<Role>> AssignRoleToUserAsync(long userId, string roleName)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task<IReadOnlyList<Role>> AssignRoleToUserAsync(long userId, string[] roleNames)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task<IReadOnlyList<Role>> GetUserRolesAsync(long userId)
    {
        throw new NotImplementedException();
    }
}
