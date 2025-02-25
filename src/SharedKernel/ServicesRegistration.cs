using SharedKernel.Interfaces;
using SharedKernel.Services;

using Microsoft.Extensions.DependencyInjection;

namespace SharedKernel;

/// <summary>
/// Provides extension methods for registering services from the SharedKernel layer
/// into the dependency injection pipeline.
/// </summary>
public static class ServicesRegistration
{
    /// <summary>
    /// Registers all services from the SharedKernel layer into the current
    /// <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">
    /// The <see cref="IServiceCollection"/> to which the SharedKernel services will be added.
    /// </param>
    /// <returns>
    /// The same <see cref="IServiceCollection"/> instance with the SharedKernel services registered.
    /// </returns>
    public static IServiceCollection AddSharedKernel(this IServiceCollection services)
    {
        services.AddScoped<IDiscountService, DiscountService>();

        return services;
    }
}
