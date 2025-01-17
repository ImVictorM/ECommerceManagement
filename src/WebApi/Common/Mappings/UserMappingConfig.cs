using Application.Common.Security.Authorization.Roles;
using Application.Users.Commands.UpdateUser;
using Application.Users.Common.DTOs;

using Contracts.Users;
using Mapster;

namespace WebApi.Common.Mappings;

/// <summary>
/// Configure mappings between user objects.
/// </summary>
public class UserMappingConfig : IRegister
{
    private static readonly Dictionary<long, Role> _roles = Role.List().ToDictionary(r => r.Id);

    /// <summary>
    /// Register the mapping configuration related to users.
    /// </summary>
    /// <param name="config">The global configuration object.</param>
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<UserResult, UserResponse>()
            .Map(dest => dest.Id, src => src.User.Id.ToString())
            .Map(dest => dest.Email, src => src.User.Email.ToString())
            .Map(dest => dest.Roles, src => GetUserRoleNames(src))
            .Map(dest => dest.Addresses, src => src.User.UserAddresses)
            .Map(dest => dest, src => src.User);

        config.NewConfig<(string UserId, UpdateUserRequest Request), UpdateUserCommand>()
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest, src => src.Request);
    }

    private static IEnumerable<string> GetUserRoleNames(UserResult userResult)
    {
        return userResult.User.UserRoles.Select(ur => _roles[ur.RoleId].Name);
    }
}
