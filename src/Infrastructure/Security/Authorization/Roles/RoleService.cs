using Application.Common.Persistence;
using Application.Common.Security.Authorization.Roles;
using Application.Common.Security.Identity;

using Domain.UserAggregate.ValueObjects;

namespace Infrastructure.Security.Authorization.Roles;

/// <summary>
/// Represents an implementation of a role service.
/// </summary>
public class RoleService : IRoleService
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="RoleService"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    public RoleService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc/>
    public bool IsAdmin(IdentityUser user)
    {
        return Role.HasAdminRole(user.Roles);
    }

    /// <inheritdoc/>
    public async Task<bool> IsAdminAsync(string id)
    {
        var user = await _unitOfWork.UserRepository.FindByIdAsync(UserId.Create(id));

        if (user == null)
        {
            return false;
        }

        var userRoleIds = user.UserRoles.Select(ur => ur.RoleId);

        return Role.HasAdminRole(userRoleIds);
    }
}
