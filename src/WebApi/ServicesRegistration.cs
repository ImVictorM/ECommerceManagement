using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using MapsterMapper;
using Mapster;
using Carter;

namespace WebApi;

/// <summary>
/// Provides extension methods for registering services from the Presentation
/// (WebApi) layer into the dependency injection pipeline.
/// </summary>
public static class ServicesRegistration
{
    /// <summary>
    /// Registers all services from the Presentation layer into the current
    /// <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">
    /// The <see cref="IServiceCollection"/> to which the Presentation services
    /// will be added.
    /// </param>
    /// <param name="configuration">
    /// The <see cref="IConfigurationManager"/> that provides access to the
    /// application's configuration settings.
    /// </param>
    /// <returns>
    /// The same <see cref="IServiceCollection"/> instance with the Presentation
    /// services registered.
    /// </returns>
    public static IServiceCollection AddPresentation(
        this IServiceCollection services,
        IConfigurationManager configuration
    )
    {
        services.AddCarter();
        services.AddHttpContextAccessor();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerDocumentation();
        services.AddAuthorization();

        services.AddProblemDetails(ConfigureProblemDetails);

        services.AddMappings();

        return services;
    }

    private static IServiceCollection AddSwaggerDocumentation(
        this IServiceCollection services
    )
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
                Description =
                "JWT Authorization header using the Bearer scheme. " +
                "Insert only your JWT Bearer token on the textbox below",
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

            setup.AddSecurityDefinition(
                jwtSecurityScheme.Reference.Id,
                jwtSecurityScheme
            );

            setup.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { jwtSecurityScheme, Array.Empty<string>() }
            });
        });

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

    private static void ConfigureProblemDetails(ProblemDetailsOptions options)
    {
        options.CustomizeProblemDetails = context =>
        {
            var traceId = context.HttpContext.TraceIdentifier;
            var userAgent = context.HttpContext.Request.Headers.UserAgent.ToString();

            context.ProblemDetails.Extensions["traceId"] = traceId;
            context.ProblemDetails.Extensions["userAgent"] = userAgent;
        };
    }
}
