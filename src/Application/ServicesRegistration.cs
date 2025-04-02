using Application.Common.Security.Authorization;
using Application.Common.Security.Authorization.Policies;
using Application.Common.Validation;
using Application.Orders.Services;
using Application.Products.Services;
using Application.Sales.Services;
using Application.Coupons.Services;
using Application.ProductReviews.Services;

using Domain.OrderAggregate.Services;
using Domain.ProductAggregate.Services;
using Domain.SaleAggregate.Services;
using Domain.CouponAggregate.Services;
using Domain.ProductReviewAggregate.Services;

using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using MediatR;
using System.Reflection;

namespace Application;

/// <summary>
/// Provides extension methods for registering services from the Application layer
/// into the dependency injection pipeline.
/// </summary>
public static class ServicesRegistration
{
    private static readonly Assembly _assembly = typeof(ServicesRegistration).Assembly;

    /// <summary>
    /// Registers all services from the Application layer into the current
    /// <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">
    /// The <see cref="IServiceCollection"/> to which the Application services
    /// will be added.
    /// </param>
    /// <returns>
    /// The same <see cref="IServiceCollection"/> instance with the Application
    /// services registered.
    /// </returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services
            .AddDomainServices()
            .AddPolicies();

        services
            .AddMediatR(configuration =>
                configuration.RegisterServicesFromAssembly(_assembly)
            )
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>))
            .AddValidatorsFromAssembly(_assembly);

        return services;
    }

    private static IServiceCollection AddDomainServices(
        this IServiceCollection services
    )
    {
        services.AddScoped<IOrderAssemblyService, OrderAssemblyService>();
        services.AddScoped<IOrderPricingService, OrderPricingService>();
        services.AddScoped<IProductPricingService, ProductPricingService>();
        services.AddScoped<IInventoryManagementService, InventoryManagementService>();
        services.AddScoped<ISaleApplicationService, SaleApplicationService>();
        services.AddScoped
            <IProductReviewEligibilityService, ProductReviewEligibilityService>();
        services.AddScoped<ISaleEligibilityService, SaleEligibilityService>();
        services.AddScoped<ICouponApplicationService, CouponApplicationService>();
        services.AddScoped<ICouponUsageService, CouponUsageService>();

        return services;
    }

    private static IServiceCollection AddPolicies(this IServiceCollection services)
    {
        services.AddScoped(typeof(SelfOrAdminPolicy<>));
        services.AddScoped(typeof(RestrictedDeactivationPolicy<>));
        services.AddScoped(typeof(RestrictedUpdatePolicy<>));
        services.AddScoped(typeof(ShipmentCarrierPolicy<>));

        return services;
    }
}
