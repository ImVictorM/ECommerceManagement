using Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.PostgreSql;

namespace IntegrationTests.Common;

/// <summary>
/// Integration tests test server.
/// </summary>
public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer;

    /// <summary>
    /// Initiates a new instance of the <see cref="IntegrationTestWebAppFactory"/> class.
    /// </summary>
    public IntegrationTestWebAppFactory()
    {
        _dbContainer = new PostgreSqlBuilder()
            .WithImage("postgres:latest")
            .Build();
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
                options.UseNpgsql(_dbContainer.GetConnectionString());
            });
        });
    }

    /// <inheritdoc/>
    public Task InitializeAsync()
    {
        return _dbContainer.StartAsync();
    }

    /// <inheritdoc/>
    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _dbContainer.DisposeAsync();
    }
}
