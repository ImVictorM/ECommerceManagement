using Application;
using Carter;
using Mapster;
using MapsterMapper;

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

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddProblemDetails(
            options => options.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Extensions["traceId"] = context.HttpContext.TraceIdentifier;
                context.ProblemDetails.Extensions["userAgent"] = context.HttpContext.Request.Headers.UserAgent.ToString();
            }
        );

        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(ApplicationAssemblyReference.Assembly);

        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();

        return services;
    }
}
