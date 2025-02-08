using Application.Common.Security.Authorization;
using Application.Common.Security.Authorization.Policies;
using Application.Common.Validation;
using Application.Orders.Services;
using Application.Products.Services;
using Application.Sales.Services;

using Domain.OrderAggregate.Services;
using Domain.ProductAggregate.Services;
using Domain.SaleAggregate.Services;

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

        services.AddScoped(typeof(SelfOrAdminPolicy<>));
        services.AddScoped(typeof(RestrictedDeactivationPolicy<>));
        services.AddScoped(typeof(RestrictedUpdatePolicy<>));
        services.AddScoped(typeof(ShipmentCarrierPolicy<>));

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));
        services.AddValidatorsFromAssembly(assembly);

        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ISaleService, SaleService>();

        return services;
    }
}
