using WebApi.Common.Interfaces;

using MapsterMapper;
using Carter;
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;

namespace WebApi;

/// <summary>
/// Add the presentation layer required services to the DI pipeline.
/// </summary>
public static class ServicesRegistration
{
    /// <summary>
    /// Add the required dependencies of the presentation layer.
    /// </summary>
    /// <param name="services">The app services.</param>
    /// <param name="configuration">The app configuration.</param>
    /// <returns>
    /// The app services including the registration of the presentation layer
    /// required services.
    /// </returns>
    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfigurationManager configuration)
    {
        services.AddCarter();
        services.AddHttpContextAccessor();
        services.AddEndpointsApiExplorer();
        services.AddSwagger();
        services.AddAuthorizationPolicies();

        services.AddProblemDetails(
            options => options.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Extensions["traceId"] = context.HttpContext.TraceIdentifier;
                context.ProblemDetails.Extensions["userAgent"] = context.HttpContext.Request.Headers.UserAgent.ToString();
            }
        );

        services.AddMappings();

        return services;
    }

    private static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(setup =>
        {
            setup.SwaggerDoc("v1", new OpenApiInfo()
            {
                Title = "ECommerce Management",
                Version = "v1",
            });

            var jwtSecurityScheme = new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Insert only your JWT Bearer token on the textbox below",
                Name = "JWT Authentication",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme,
                }
            };

            setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

            setup.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { jwtSecurityScheme, Array.Empty<string>() }
            });
        });

        return services;
    }

    private static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
    {
        // Configure policies
        services.AddAuthorization(options =>
        {
            var assembly = typeof(ServicesRegistration).Assembly;

            var policyTypes = assembly.GetTypes()
                .Where(type => typeof(IAuthorizationPolicy).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);

            foreach (var policyType in policyTypes)
            {
                if (Activator.CreateInstance(policyType) is IAuthorizationPolicy policy)
                {
                    policy.ConfigurePolicy(options);
                }
            }
        });

        // Configure authorization handler
        var handlerTypes = typeof(ServicesRegistration).Assembly.GetTypes()
            .Where(type => typeof(IAuthorizationHandler).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);

        foreach (var handlerType in handlerTypes)
        {
            services.AddScoped(typeof(IAuthorizationHandler), handlerType);
        }

        return services;
    }

    private static IServiceCollection AddMappings(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(typeof(ServicesRegistration).Assembly);

        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();

        return services;
    }
}
