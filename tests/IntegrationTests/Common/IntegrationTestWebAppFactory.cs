using Infrastructure.Persistence;
using IntegrationTests.TestUtils.Seeds;
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

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ECommerceDbContext>();

            SeedDatabase(dbContext);
        });
    }

    /// <summary>
    /// Seeds the database with initial data.
    /// </summary>
    /// <param name="dbContext">The db context.</param>
    private static void SeedDatabase(ECommerceDbContext dbContext)
    {
        dbContext.Database.EnsureCreated();

        dbContext.Users.AddRange(UserSeed.ListUsers());

        dbContext.SaveChanges();
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
