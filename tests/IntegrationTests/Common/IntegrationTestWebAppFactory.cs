using System.Data.Common;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using Respawn;
using Testcontainers.PostgreSql;

namespace IntegrationTests.Common;

/// <summary>
/// Integration tests test server.
/// </summary>
public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private PostgreSqlContainer _dbContainer = null!;
    private Respawner _respawner = null!;
    private DbConnection _dbConnection = null!;

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
        _dbContainer = new PostgreSqlBuilder()
           .WithImage("postgres:latest")
           .WithDatabase("ecommerce-management-test")
           .WithPortBinding("8002", "5432")
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
        });
    }

    private async Task CreateDatabaseAsync()
    {
        using var scope = Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<ECommerceDbContext>();

        await dbContext.Database.EnsureCreatedAsync();
    }
}
