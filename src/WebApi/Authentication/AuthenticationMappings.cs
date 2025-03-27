using Application.Authentication.Commands.RegisterCustomer;
using Application.Authentication.Queries.LoginUser;
using Application.Authentication.DTOs.Results;

using Contracts.Authentication;

using Mapster;

namespace WebApi.Authentication;

internal sealed class AuthenticationMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterCustomerRequest, RegisterCustomerCommand>();
        config.NewConfig<LoginUserRequest, LoginUserQuery>();
        config.NewConfig<AuthenticationResult, AuthenticationResponse>()
            .Map(dest => dest, src => src.User)
            .Map(dest => dest.Token, src => src.Token);
    }
}
