using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Infrastructure.Authentication;
using Infrastructure.Persistence;
using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Authentication;

namespace Infrastructure;

/// <summary>
/// Add the infrastructure layer required services to the DI pipeline.
/// </summary>
public static class ServicesRegistration
{
    /// <summary>
    /// Add the required dependencies of the infrastructure layer.
    /// </summary>
    /// <param name="services">The app services.</param>
    /// <param name="configuration">The app configurations.</param>
    /// <returns>
    /// The app services including the registration of the infrastructure layer
    /// required services.
    /// </returns>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfigurationManager configuration
    )
    {
        services.Configure<JwtSettings>(configuration?.GetSection(JwtSettings.SectionName)!);
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}
