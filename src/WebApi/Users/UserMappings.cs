using Application.Users.Commands.UpdateUser;
using Application.Users.DTOs.Results;

using Contracts.Users;

using Mapster;

namespace WebApi.Users;

internal sealed class UserMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<UserResult, UserResponse>();

        config
            .NewConfig<(string Id, UpdateUserRequest Request), UpdateUserCommand>()
            .Map(dest => dest.UserId, src => src.Id)
            .Map(dest => dest, src => src.Request);
    }
}
