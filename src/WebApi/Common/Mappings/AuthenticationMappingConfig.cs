using Application.Authentication.Commands.Register;
using Application.Authentication.Common;
using Application.Authentication.Queries.Login;
using Contracts.Authentication;
using Mapster;

namespace WebApi.Common.Mappings;

/// <summary>
/// Configure mappings between authentication objects.
/// </summary>
public class AuthenticationMappingConfig : IRegister
{
    /// <summary>
    /// Register the mapping configurations.
    /// </summary>
    /// <param name="config">Global configuration object.</param>
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterRequest, RegisterCommand>();
        config.NewConfig<LoginRequest, LoginQuery>();
        config.NewConfig<AuthenticationResult, AuthenticationResponse>()
            .Map(dest => dest.Id, src => src.User.Id.Value)
            .Map(dest => dest, src => src.User)
            .Map(dest => dest.Email, src => src.User.Email.Value);
    }
}
