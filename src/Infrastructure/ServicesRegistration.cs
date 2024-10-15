using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Infrastructure.Authentication;
using Infrastructure.Persistence;
using Application.Common.Interfaces.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Infrastructure.Persistence.Interceptors;
using Application.Common.Interfaces.Persistence;
using Microsoft.EntityFrameworkCore;

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
        var dbConnectionSettings = new DbConnectionSettings();
        configuration.Bind(DbConnectionSettings.SectionName, dbConnectionSettings);

        services.AddSingleton(Options.Create(dbConnectionSettings));

        services.AddDbContext<ECommerceDbContext>(options =>
        {
            options.UseNpgsql($"Host={dbConnectionSettings.Host};Port={dbConnectionSettings.Port};Database={dbConnectionSettings.Database};Username={dbConnectionSettings.Username};Password={dbConnectionSettings.Password};Trust Server Certificate=true;");
        });

        services.AddAuth(configuration);

        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<AuditInterceptor>();
        services.AddScoped<PublishDomainEventsInterceptor>();

        return services;
    }

    /// <summary>
    /// Add the required dependencies for the authentication process.
    /// </summary>
    /// <param name="services">The application services.</param>
    /// <param name="configuration">The application configuration.</param>
    /// <returns>The application services including the registration of the authentication services.</returns>
    public static IServiceCollection AddAuth(
        this IServiceCollection services,
        IConfigurationManager configuration
    )
    {
        var jwtSettings = new JwtSettings();
        configuration.Bind(JwtSettings.SectionName, jwtSettings);

        services.AddSingleton(Options.Create(jwtSettings));
        services.AddScoped<IJwtTokenService, JwtTokenService>();

        services
            .AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
            });

        services.AddAuthorization();

        return services;
    }
}
