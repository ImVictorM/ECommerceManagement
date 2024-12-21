using Application.Common.Behaviors;
using Application.Orders.Services;
using Domain.OrderAggregate.Services;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

/// <summary>
/// Add the application layer required services to the DI pipeline.
/// </summary>
public static class ServicesRegistration
{
    /// <summary>
    /// Add the required dependencies of the application layer.
    /// </summary>
    /// <param name="services">The app services.</param>
    /// <returns>
    /// The app services including the registration of the application layer
    /// required services.
    /// </returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(ServicesRegistration).Assembly;

        services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(assembly));

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddScoped<IOrderProductService, OrderProductServices>();
        services.AddScoped<IOrderAccessService, OrderAccessServices>();

        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}
