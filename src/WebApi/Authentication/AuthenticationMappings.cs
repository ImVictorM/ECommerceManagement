using Application.Authentication.Commands.RegisterCustomer;
using Application.Authentication.Queries.LoginUser;
using Application.Authentication.DTOs;

using Contracts.Authentication;

using Mapster;

namespace WebApi.Authentication;

internal sealed class AuthenticationMappings : IRegister
{
    /// <inheritdoc/>
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterCustomerRequest, RegisterCustomerCommand>();
        config.NewConfig<LoginUserRequest, LoginUserQuery>();
        config.NewConfig<AuthenticationResult, AuthenticationResponse>()
            .Map(dest => dest.Id, src => src.AuthenticatedIdentity.Id.ToString())
            .Map(dest => dest, src => src.AuthenticatedIdentity)
            .Map(dest => dest.Email, src => src.AuthenticatedIdentity.Email.ToString());
    }
}
