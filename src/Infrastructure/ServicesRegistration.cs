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
using Microsoft.Extensions.Hosting;
using Application.Common.Interfaces.Services;
using Infrastructure.Payments;

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
    /// <param name="environment">The host environment.</param>
    /// <returns>
    /// The app services including the registration of the infrastructure layer
    /// required services.
    /// </returns>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfigurationManager configuration,
        IHostEnvironment environment
    )
    {
        var dbConnectionSettings = new DbConnectionSettings();
        configuration.Bind(DbConnectionSettings.SectionName, dbConnectionSettings);

        services.AddSingleton(Options.Create(dbConnectionSettings));

        services.AddDbContext<ECommerceDbContext>(options =>
        {
            if (!environment.IsProduction())
            {
                options.EnableSensitiveDataLogging();
            }

            options.UseNpgsql($"Host={dbConnectionSettings.Host};Port={dbConnectionSettings.Port};Database={dbConnectionSettings.Database};Username={dbConnectionSettings.Username};Password={dbConnectionSettings.Password};Trust Server Certificate=true;");
        });

        services.AddAuth(configuration);

        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IPaymentGateway, MockPaymentGateway>();

        services.AddScoped<AuditInterceptor>();
        services.AddScoped<PublishDomainEventsInterceptor>();

        return services;
    }

    private static IServiceCollection AddAuth(
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
            .AddJwtBearer(options =>
            {
                options.MapInboundClaims = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                };
            });

        return services;
    }
}
