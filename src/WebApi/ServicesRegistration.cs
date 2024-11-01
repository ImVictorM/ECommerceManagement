using Carter;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using WebApi.Common.Interfaces;

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
    /// <returns>
    /// The app services including the registration of the presentation layer
    /// required services.
    /// </returns>
    public static IServiceCollection AddPresentation(this IServiceCollection services)
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

    /// <summary>
    /// Configures swagger and add it to the DI pipeline.
    /// </summary>
    /// <param name="services">The application services.</param>
    /// <returns>The application services including the registration of the swagger services.</returns>
    public static IServiceCollection AddSwagger(this IServiceCollection services)
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

    /// <summary>
    /// Configures authorization policies and add it to the DI pipeline.
    /// </summary>
    /// <param name="services">The application services.</param>
    /// <returns>The application services including the registration of policy-related services.</returns>
    public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
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
            services.AddSingleton(typeof(IAuthorizationHandler), handlerType);
        }

        return services;
    }

    /// <summary>
    /// Add mapping related services.
    /// </summary>
    /// <param name="services">The application services.</param>
    /// <returns>The application services including the registration of mapping services.</returns>
    public static IServiceCollection AddMappings(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(typeof(ServicesRegistration).Assembly);

        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();

        return services;
    }
}
