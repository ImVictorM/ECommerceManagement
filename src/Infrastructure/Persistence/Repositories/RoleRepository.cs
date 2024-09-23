using Application.Common.Interfaces.Persistence;
using Domain.RoleAggregate;

namespace Infrastructure.Persistence.Repositories;

public class RoleRepository : IRoleRepository
{
    public Task<IReadOnlyList<Role>> AssignRoleToUserAsync(long userId, string roleName)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<Role>> AssignRoleToUserAsync(long userId, string[] roleNames)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<Role>> GetUserRolesAsync(long userId)
    {
        throw new NotImplementedException();
    }
}
