using Application.Common.Security.Authentication;
using Application.Common.Persistence;
using Application.Common.Security.Authorization;
using Application.Common.PaymentGateway;
using Application.Common.Security.Identity;

using Infrastructure.Security.Identity;
using Infrastructure.PaymentGateway;
using Infrastructure.Security.Authorization;
using Infrastructure.Security.Authentication;
using Infrastructure.Security.Authentication.Settings;
using Infrastructure.Common.Persistence;
using Infrastructure.Common.Persistence.Interceptors;
using Infrastructure.Users;
using Infrastructure.Carriers;
using Infrastructure.Common.Persistence.Configurations.Abstracts;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Reflection;

namespace Infrastructure;

/// <summary>
/// Add the infrastructure layer required services to the DI pipeline.
/// </summary>
public static class ServicesRegistration
{
    private static readonly Assembly _assembly = typeof(ServicesRegistration).Assembly;

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
        services.AddPersistence(configuration, environment);

        services.AddScoped<IPaymentGateway, MockPaymentGateway>();
        services.Configure<CarrierInternalSettings>(configuration.GetSection(CarrierInternalSettings.SectionName));
        services.Configure<AdminAccountSettings>(configuration.GetSection(AdminAccountSettings.SectionName));

        services.AddSecurity(configuration);

        return services;
    }

    private static IServiceCollection AddPersistence(
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

        var repositoryTypes = _assembly.DefinedTypes
            .Where(t =>
                !t.IsAbstract
                && !t.IsGenericTypeDefinition
                && t.GetInterfaces().Any(i => !i.IsGenericTypeDefinition && i.IsAssignableFrom(typeof(IBaseRepository<,>)))
            )
            .Select(typeInfo => typeInfo.AsType());

        foreach (var repositoryType in repositoryTypes)
        {
            var repositoryInterface = repositoryType
                .GetInterfaces()
                .First(i => !i.IsGenericType && i.IsAssignableFrom(typeof(IBaseRepository<,>)));

            services.AddScoped(repositoryInterface, repositoryType);
        }

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<AuditInterceptor>();
        services.AddScoped<PublishDomainEventsInterceptor>();

        foreach
        (var type in typeof(ECommerceDbContext).Assembly.DefinedTypes
            .Where(t =>
                !t.IsAbstract
                && !t.IsGenericTypeDefinition
                && typeof(EntityTypeConfigurationDependency).IsAssignableFrom(t)
            )
        )
        {
            services.AddScoped(typeof(EntityTypeConfigurationDependency), type);
        }

        return services;
    }

    private static IServiceCollection AddSecurity(
        this IServiceCollection services,
        IConfigurationManager configuration
    )
    {
        var jwtSettings = new JwtSettings();
        configuration.Bind(JwtSettings.SectionName, jwtSettings);

        services.AddSingleton(Options.Create(jwtSettings));
        services.Configure<HmacSignatureSettings>(configuration.GetSection(HmacSignatureSettings.SectionName));

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

        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IHmacSignatureProvider, HmacSignatureProvider>();
        services.AddScoped<IIdentityProvider, IdentityProvider>();
        services.AddScoped<IAuthorizationService, AuthorizationService>();

        return services;
    }
}
