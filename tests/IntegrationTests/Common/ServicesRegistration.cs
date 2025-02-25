using Application.Common.Security.Authentication;

using Infrastructure.Common.Persistence;
using Infrastructure.Security.Authentication;
using Infrastructure.Security.Authentication.Settings;

using SharedKernel.Interfaces;
using SharedKernel.Services;

using IntegrationTests.Common.Requests.Abstracts;
using IntegrationTests.Common.Requests;
using IntegrationTests.Common.Seeds;
using IntegrationTests.Common.Seeds.Abstracts;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Data.Common;
using System.Reflection;

namespace IntegrationTests.Common;

/// <summary>
/// Register the integration test required services.
/// </summary>
public static class ServicesRegistration
{
    private static Assembly _assembly => typeof(ServicesRegistration).Assembly;

    /// <summary>
    /// Adds the required dependencies for the integration tests.
    /// </summary>
    /// <typeparam name="TStartup">The entry point.</typeparam>
    /// <param name="services">The current services.</param>
    /// <param name="configurations">The configurations.</param>
    /// <param name="connection">The database connection.</param>
    /// <param name="appFactory">The app factory.</param>
    /// <returns>
    /// The services including the registration of the integration test
    /// required services.
    /// </returns>
    public static IServiceCollection AddTestServices<TStartup>(
        this IServiceCollection services,
        IConfiguration configurations,
        DbConnection connection,
        WebApplicationFactory<TStartup> appFactory
    )
        where TStartup : class
    {
        services
            .AddTestDbContext(connection)
            .AddSingleton<IHttpClientFactory>(new TestHttpClientFactory<TStartup>(appFactory))
            .AddSingleton<IPasswordHasher, PasswordHasher>()
            .AddSeed()
            .AddCredentialsProviders()
            .AddHmacSignatureProvider(configurations);

        services.AddTransient<IDiscountService, DiscountService>();
        services.AddTransient<IRequestService, RequestService>();

        return services;
    }

    private static IServiceCollection AddTestDbContext(this IServiceCollection services, DbConnection connection)
    {
        services.RemoveAll(typeof(DbContextOptions<ECommerceDbContext>));

        services.AddDbContext<ECommerceDbContext>(options =>
        {
            options.UseNpgsql(connection);
        });

        return services;
    }

    private static IServiceCollection AddSeed(this IServiceCollection services)
    {
        var seedTypes = _assembly.DefinedTypes
                .Where(t =>
                    !t.IsAbstract
                    && !t.IsGenericTypeDefinition
                    && typeof(ISeed).IsAssignableFrom(t)
                )
                .Select(typeInfo => typeInfo.AsType())
                .ToList();

        foreach (var type in seedTypes)
        {
            services.AddSingleton(type);
            services.TryAddEnumerable(ServiceDescriptor.Singleton(typeof(ISeed), type));

            var dataSeedInterfaceType = type
                .GetInterfaces()
                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDataSeed<,>));

            services.AddSingleton(dataSeedInterfaceType, sp => sp.GetRequiredService(type));
        }

        services.AddSingleton<ISeedManager, SeedManager>();

        return services;
    }

    private static IServiceCollection AddCredentialsProviders(this IServiceCollection services)
    {
        var credentialsProviderTypes = _assembly.DefinedTypes
                .Where(t =>
                    !t.IsAbstract
                    && !t.IsGenericTypeDefinition
                    && t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICredentialsProvider<>)))
                .Select(t => t.AsType())
                .ToList();

        foreach (var type in credentialsProviderTypes)
        {
            var credentialsProviderInterfaceType = type
                .GetInterfaces()
                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICredentialsProvider<>));

            services.AddSingleton(credentialsProviderInterfaceType, type);
        }

        return services;
    }

    private static IServiceCollection AddHmacSignatureProvider(this IServiceCollection services, IConfiguration configurations)
    {
        var hmacSignatureSettings = new HmacSignatureSettings();
        configurations.Bind(HmacSignatureSettings.SectionName, hmacSignatureSettings);

        services.AddSingleton<IHmacSignatureProvider>(new HmacSignatureProvider(Options.Create(hmacSignatureSettings)));

        return services;
    }
}
