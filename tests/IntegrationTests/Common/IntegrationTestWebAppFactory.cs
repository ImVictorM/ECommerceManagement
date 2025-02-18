using Infrastructure.Common.Persistence;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
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
            services.AddTestServices(_configuration, _dbConnection, this);
        });
    }

    private async Task CreateDatabaseAsync()
    {
        using var scope = Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<ECommerceDbContext>();

        await dbContext.Database.EnsureCreatedAsync();
    }
}
