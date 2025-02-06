using Application.Authentication.Commands.RegisterCustomer;
using Application.Authentication.Queries.LoginUser;
using Application.Authentication.DTOs;

using Contracts.Authentication;

using Mapster;

namespace WebApi.Authentication;

/// <summary>
/// Configures the mappings between authentication objects.
/// </summary>
public class AuthenticationMappings : IRegister
{
    /// <inheritdoc/>
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterCustomerRequest, RegisterCustomerCommand>();
        config.NewConfig<LoginUserRequest, LoginUserQuery>();
        config.NewConfig<AuthenticationResult, AuthenticationResponse>()
            .Map(dest => dest.Id, src => src.User.Id.ToString())
            .Map(dest => dest, src => src.User)
            .Map(dest => dest.Email, src => src.User.Email.ToString());
    }
}
