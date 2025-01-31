using Application.Common.Extensions;
using Application.Users.Commands.UpdateUser;
using Application.Users.DTOs;

using Contracts.Users;

using Mapster;

namespace WebApi.Users;

/// <summary>
/// Configures the mappings between user objects.
/// </summary>
public class UserMappings : IRegister
{
    /// <inheritdoc/>
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<UserResult, UserResponse>()
            .Map(dest => dest.Id, src => src.User.Id.ToString())
            .Map(dest => dest.Email, src => src.User.Email.ToString())
            .Map(dest => dest.Roles, src => src.User.UserRoles.GetRoleNames())
            .Map(dest => dest.Addresses, src => src.User.UserAddresses)
            .Map(dest => dest, src => src.User);

        config.NewConfig<(string UserId, UpdateUserRequest Request), UpdateUserCommand>()
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest, src => src.Request);
    }
}
