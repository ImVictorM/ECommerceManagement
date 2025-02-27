using Infrastructure.Common.Persistence;

using IntegrationTests.Common.Requests.Abstracts;
using IntegrationTests.Common.Seeds.Abstracts;

using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace IntegrationTests.Common;

/// <summary>
/// Base integration test that uses the shared database and has a pre-configured setup and teardown for each test.
/// </summary>
[Collection(nameof(SharedDatabaseCollectionFixture))]
public class BaseIntegrationTest : IAsyncLifetime
{
    private readonly IntegrationTestWebAppFactory _factory;

    /// <summary>
    /// Gets a helper to log some data to the console.
    /// </summary>
    public ITestOutputHelper Output { get; }

    /// <summary>
    /// Gets the seed manager for test data initialization.
    /// </summary>
    public ISeedManager SeedManager { get; }

    /// <summary>
    /// Gets the request service.
    /// </summary>
    public IRequestService RequestService { get; }

    /// <summary>
    /// Gets the link generator;
    /// </summary>
    public LinkGenerator LinkGenerator { get; }

    /// <summary>
    /// Initiates a new instance of the <see cref="BaseIntegrationTest"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">A log helper.</param>
    public BaseIntegrationTest(IntegrationTestWebAppFactory factory, ITestOutputHelper output)
    {
        _factory = factory;
        Output = output;

        using var scope = _factory.Services.CreateScope();
        SeedManager = scope.ServiceProvider.GetRequiredService<ISeedManager>();
        RequestService = scope.ServiceProvider.GetRequiredService<IRequestService>();
        LinkGenerator = scope.ServiceProvider.GetRequiredService<LinkGenerator>();
    }

    /// <inheritdoc/>
    public async Task InitializeAsync()
    {
        await _factory.ResetDatabaseAsync();
        await SeedDataAsync();
    }

    /// <inheritdoc/>
    public async Task DisposeAsync()
    {
        await _factory.ResetDatabaseAsync();
    }

    private async Task SeedDataAsync()
    {
        using var scope = _factory.Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<IECommerceDbContext>();

        await SeedManager.SeedAsync(dbContext);
    }
}
