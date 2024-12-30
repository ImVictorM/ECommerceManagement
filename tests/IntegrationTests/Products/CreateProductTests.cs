using IntegrationTests.Common;
using IntegrationTests.Products.TestUtils;
using IntegrationTests.TestUtils.Extensions.HttpClient;
using IntegrationTests.TestUtils.Extensions.Products;
using IntegrationTests.TestUtils.Seeds;

using Contracts.Products;

using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Xunit.Abstractions;

namespace IntegrationTests.Products;

/// <summary>
/// Integration tests for the product creation, covering authentication and authorization scenarios,
/// request validation, and expected responses when attempting to create products under different conditions.
/// </summary>
public class CreateProductTests : BaseIntegrationTest
{
    /// <summary>
    /// Initiates a new instance of the <see cref="CreateProductTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public CreateProductTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output) : base(factory, output)
    {
    }

    /// <summary>
    /// Tests that when an authenticated user without admin privileges tries to create a product, the response
    /// status is Forbidden. Ensures that only admins are allowed to create products.
    /// </summary>
    /// <param name="customerType">The type of non-admin user attempting to create a product.</param>
    [Theory]
    [InlineData(SeedAvailableUsers.CustomerWithAddress)]
    [InlineData(SeedAvailableUsers.Customer)]
    public async Task CreateProduct_WhenUserAuthenticatedIsNotAdmin_ReturnsForbidden(SeedAvailableUsers customerType)
    {
        var request = CreateProductRequestUtils.CreateRequest();

        await Client.LoginAs(customerType);
        var response = await Client.PostAsJsonAsync("/products", request);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Tests that when an unauthenticated user attempts to create a product, the response status is Unauthorized.
    /// Ensures that only authenticated users with the required admin role can create products.
    /// </summary>
    [Fact]
    public async Task CreateProduct_WhenUserIsNotAuthenticated_ReturnsUnauthorize()
    {
        var request = CreateProductRequestUtils.CreateRequest();

        var response = await Client.PostAsJsonAsync("/products", request);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Tests that when an authenticated admin user creates a product, the product is created successfully and the
    /// response status is Created. Confirms that admins have permission to create products.
    /// </summary>
    [Fact]
    public async Task CreateProduct_WhenUserAuthenticatedIsAdmin_CreatesProductAndReturnsCreated()
    {
        var productCategories = new[]
        {
            CategorySeed.GetSeedCategory(SeedAvailableCategories.BOOKS_STATIONERY),
            CategorySeed.GetSeedCategory(SeedAvailableCategories.TECHNOLOGY)
        };

        var request = CreateProductRequestUtils.CreateRequest(
            categoryIds: productCategories.Select(c => c.Id.ToString())
        );

        await Client.LoginAs(SeedAvailableUsers.Admin);

        var postResponse = await Client.PostAsJsonAsync("/products", request);
        var resourceLocation = postResponse.Headers.Location;
        var getResponse = await Client.GetAsync(resourceLocation);
        var getResponseContent = await getResponse.Content.ReadFromJsonAsync<ProductResponse>();

        postResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        getResponseContent!.EnsureCreatedFromRequest(request);
        getResponseContent!.Categories.Should().BeEquivalentTo(productCategories.Select(c => c.Name));
    }
}
