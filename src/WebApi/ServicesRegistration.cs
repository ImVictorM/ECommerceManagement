using Application.Common.Constants.Policies;
using Carter;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
    /// <returns>
    /// The app services including the registration of the presentation layer
    /// required services.
    /// </returns>
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddCarter();
        services.AddHttpContextAccessor();
        services.AddEndpointsApiExplorer();
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

        services.AddProblemDetails(
            options => options.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Extensions["traceId"] = context.HttpContext.TraceIdentifier;
                context.ProblemDetails.Extensions["userAgent"] = context.HttpContext.Request.Headers.UserAgent.ToString();
            }
        );

        services.AddAuthorizationBuilder()
            .AddPolicy(PolicyConstants.Admin.Name, policy => policy.RequireRole(PolicyConstants.Admin.RoleName))
            .AddPolicy(PolicyConstants.Customer.Name, policy => policy.RequireRole(PolicyConstants.Customer.RoleName));

        services.AddMappings();

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
