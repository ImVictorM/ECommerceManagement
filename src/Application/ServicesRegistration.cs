using Application.Common.Security.Authorization;
using Application.Common.Security.Authorization.Policies;
using Application.Common.Validation;
using Application.Orders.Services;
using Application.Products.Services;
using Application.Sales.Services;
using Application.ProductFeedback.Services;

using Domain.OrderAggregate.Services;
using Domain.ProductAggregate.Services;
using Domain.SaleAggregate.Services;
using Domain.ProductFeedbackAggregate.Services;

using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using MediatR;

namespace Application;

/// <summary>
/// Provides extension methods for registering services from the Application layer
/// into the dependency injection pipeline.
/// </summary>
public static class ServicesRegistration
{
    /// <summary>
    /// Registers all services from the Application layer into the current
    /// <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">
    /// The <see cref="IServiceCollection"/> to which the Application services will be added.
    /// </param>
    /// <returns>
    /// The same <see cref="IServiceCollection"/> instance with the Application services registered.
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
        services.AddScoped<IProductFeedbackService, ProductFeedbackService>();

        return services;
    }
}
