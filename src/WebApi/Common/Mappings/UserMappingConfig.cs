using Application.Users.Queries.GetUserById;
using Contracts.Users;
using Mapster;

namespace WebApi.Common.Mappings;

/// <summary>
/// Configure mappings between user objects.
/// </summary>
public class UserMappingConfig : IRegister
{
    /// <summary>
    /// Register the mapping configuration related to users.
    /// </summary>
    /// <param name="config">The global configuration object.</param>
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<GetUserByIdResult, UserByIdResponse>()
            .Map(dest => dest.Id, src => src.User.Id.ToString())
            .Map(dest => dest.Email, src => src.User.Email.ToString())
            .Map(dest => dest.Roles, src => src.User.GetRoleNames())
            .Map(dest => dest.Addresses, src => src.User.UserAddresses)
            .Map(dest => dest, src => src.User);
    }
}
