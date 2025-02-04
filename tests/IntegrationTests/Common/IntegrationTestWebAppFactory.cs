using Infrastructure.Common.Persistence;

using IntegrationTests.Common.Seeds;
using IntegrationTests.Common.Seeds.Abstracts;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Configuration;
using Testcontainers.PostgreSql;
using System.Data.Common;
using Npgsql;
using Respawn;

namespace IntegrationTests.Common;

/// <summary>
/// Integration tests test server.
/// </summary>
public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private PostgreSqlContainer _dbContainer = null!;
    private Respawner _respawner = null!;
    private DbConnectionSettings _connectionSettings = null!;
    private DbConnection _dbConnection = null!;
    private IConfiguration _configuration = null!;

    /// <summary>
    /// Uses respawn to reset the database.
    /// </summary>
    public async Task ResetDatabaseAsync()
    {
        if (_dbConnection.State != System.Data.ConnectionState.Open)
        {
            await _dbConnection.OpenAsync();
        }

        await _respawner.ResetAsync(_dbConnection);
    }

    /// <inheritdoc/>
    public async Task InitializeAsync()
    {
        _configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Test.json")
            .AddEnvironmentVariables()
            .Build();

        _connectionSettings = new DbConnectionSettings();

        _configuration.Bind(DbConnectionSettings.SectionName, _connectionSettings);

        _dbContainer = new PostgreSqlBuilder()
           .WithImage("postgres:latest")
           .WithDatabase(_connectionSettings.Database)
           .WithPortBinding(_connectionSettings.Port, "5432")
           .WithHostname(_connectionSettings.Host)
           .WithUsername(_connectionSettings.Username)
           .WithPassword(_connectionSettings.Password)
           .Build();

        await _dbContainer.StartAsync();

        _dbConnection = new NpgsqlConnection(_dbContainer.GetConnectionString());

        await CreateDatabaseAsync();

        await _dbConnection.OpenAsync();

        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            WithReseed = false,
            TablesToIgnore =
            [
                "roles",
                "order_statuses",
                "shipment_statuses",
                "payment_statuses"
            ],
        });
    }

    /// <inheritdoc/>
    public new async Task DisposeAsync()
    {
        await _dbConnection.CloseAsync();
        await _dbConnection.DisposeAsync();
        await _dbContainer.StopAsync();
        await _dbContainer.DisposeAsync();
    }

    /// <inheritdoc/>
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<ECommerceDbContext>));

            services.AddDbContext<ECommerceDbContext>(options =>
            {
                options.UseNpgsql(_dbConnection);
            });

            var assembly = typeof(IntegrationTestWebAppFactory).Assembly;

            var seedTypes = assembly.DefinedTypes
                .Where(t =>
                    !t.IsAbstract
                    && !t.IsGenericTypeDefinition
                    && typeof(ISeed).IsAssignableFrom(t)
                )
                .Select(typeInfo => typeInfo.AsType())
                .ToList();

            foreach (var type in seedTypes)
            {
                services.AddSingleton(type, type);
                services.TryAddEnumerable(ServiceDescriptor.Singleton(typeof(ISeed), type));

                var dataSeedType = type
                    .GetInterfaces()
                    .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDataSeed<,>));

                services.AddSingleton(dataSeedType, sp => sp.GetRequiredService(type));
            }

            services.AddSingleton<ISeedManager, SeedManager>();
        });
    }

    private async Task CreateDatabaseAsync()
    {
        using var scope = Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<ECommerceDbContext>();

        await dbContext.Database.EnsureCreatedAsync();
    }
}
