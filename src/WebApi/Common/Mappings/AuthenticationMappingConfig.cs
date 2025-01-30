using Application.Authentication.Commands.Register;
using Application.Authentication.DTOs;
using Application.Authentication.Queries.Login;

using Contracts.Authentication;

using Mapster;

namespace WebApi.Common.Mappings;

/// <summary>
/// Configures the mappings between authentication objects.
/// </summary>
public class AuthenticationMappingConfig : IRegister
{
    /// <inheritdoc/>
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterRequest, RegisterCommand>();
        config.NewConfig<LoginRequest, LoginQuery>();
        config.NewConfig<AuthenticationResult, AuthenticationResponse>()
            .Map(dest => dest.Id, src => src.User.Id.ToString())
            .Map(dest => dest, src => src.User)
            .Map(dest => dest.Email, src => src.User.Email.ToString());
    }
}
