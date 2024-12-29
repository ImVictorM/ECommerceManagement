using Infrastructure.Persistence;
using IntegrationTests.TestUtils.Seeds;
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
    /// Gets an HTTP client to make requests.
    /// </summary>
    public HttpClient Client { get; init; }

    /// <summary>
    /// Gets a helper to log some data to the console.
    /// </summary>
    public ITestOutputHelper Output { get; init; }

    /// <summary>
    /// Initiates a new instance of the <see cref="BaseIntegrationTest"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">A log helper.</param>
    public BaseIntegrationTest(IntegrationTestWebAppFactory factory, ITestOutputHelper output)
    {
        _factory = factory;
        Output = output;
        Client = factory.CreateClient();
    }

    /// <inheritdoc/>
    public async Task InitializeAsync()
    {
        await SeedDataAsync();
    }

    /// <inheritdoc/>
    public async Task DisposeAsync()
    {
        await _factory.ResetDatabaseAsync();
    }

    /// <summary>
    /// Seeds the database.
    /// </summary>
    public async Task SeedDataAsync()
    {
        using var scope = _factory.Services.CreateAsyncScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<ECommerceDbContext>();

        await dbContext.Categories.AddRangeAsync(CategorySeed.ListCategories());
        await dbContext.Users.AddRangeAsync(UserSeed.ListUsers());
        await dbContext.Products.AddRangeAsync(ProductSeed.ListProducts());

        await dbContext.SaveChangesAsync();
    }
}
