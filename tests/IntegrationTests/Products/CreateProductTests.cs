using Contracts.Products;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.Common.Seeds.Categories;
using IntegrationTests.Products.TestUtils;
using IntegrationTests.TestUtils.Extensions.Products;
using IntegrationTests.TestUtils.Extensions.Http;

using WebApi.Products;

using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Xunit.Abstractions;
using Microsoft.AspNetCore.Routing;

namespace IntegrationTests.Products;

/// <summary>
/// Integration tests for the create product feature.
/// </summary>
public class CreateProductTests : BaseIntegrationTest
{
    private readonly ICategorySeed _seedCategory;
    private readonly string? _endpoint;

    /// <summary>
    /// Initiates a new instance of the <see cref="CreateProductTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public CreateProductTests(
        IntegrationTestWebAppFactory factory,
        ITestOutputHelper output
    ) : base(factory, output)
    {
        _seedCategory = SeedManager.GetSeed<ICategorySeed>();

        _endpoint = LinkGenerator.GetPathByName(
            nameof(ProductEndpoints.CreateProduct)
        );
    }

    /// <summary>
    /// Tests that when an authenticated user without admin privileges tries
    /// to create a product, the response status is Forbidden.
    /// Ensures that only admins are allowed to create products.
    /// </summary>
    /// <param name="customerType">
    /// The type of non-admin user attempting to create a product.
    /// </param>
    [Theory]
    [InlineData(UserSeedType.CUSTOMER_WITH_ADDRESS)]
    [InlineData(UserSeedType.CUSTOMER)]
    public async Task CreateProduct_WhenUserAuthenticatedIsNotAdmin_ReturnsForbidden(
        UserSeedType customerType
    )
    {
        var request = CreateProductRequestUtils.CreateRequest();

        var client = await RequestService.LoginAsAsync(customerType);
        var response = await client.PostAsJsonAsync(
            _endpoint,
            request
        );

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Tests that when an unauthenticated user attempts to create a product,
    /// the response status is Unauthorized. Ensures that only authenticated
    /// users with the required admin role can create products.
    /// </summary>
    [Fact]
    public async Task CreateProduct_WhenUserIsNotAuthenticated_ReturnsUnauthorize()
    {
        var request = CreateProductRequestUtils.CreateRequest();

        var response = await RequestService.CreateClient().PostAsJsonAsync(
            _endpoint,
            request
        );

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Tests that when an authenticated admin user creates a product,
    /// the product is created successfully and the response status
    /// is Created. Confirms that admins have permission to create products.
    /// </summary>
    [Fact]
    public async Task CreateProduct_WhenUserAuthenticatedIsAdmin_ReturnsCreated()
    {
        var productCategories = new[]
        {
           _seedCategory.GetEntity(CategorySeedType.BOOKS_STATIONERY),
            _seedCategory.GetEntity(CategorySeedType.TECHNOLOGY)
        };

        var productCategoryIds = productCategories.Select(c => c.Id.ToString());
        var productCategoryNames = productCategories.Select(c => c.Name);

        var request = CreateProductRequestUtils.CreateRequest(
            name: "Techy pen",
            description: "very cool, you should buy it",
            initialQuantity: 10,
            basePrice: 2000m,
            categoryIds: productCategoryIds,
            images:
            [
                new Uri("pen-photo.png", UriKind.Relative)
            ]
        );

        var client = await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var responseCreate = await client.PostAsJsonAsync(
            _endpoint,
            request
        );
        var createdResourceLocation = responseCreate.Headers.Location;

        var responseGetCreated = await client.GetAsync(
            createdResourceLocation
        );

        var responseGetCreatedContent = await responseGetCreated.Content
            .ReadRequiredFromJsonAsync<ProductResponse>();

        responseCreate.StatusCode.Should().Be(HttpStatusCode.Created);
        responseGetCreated.StatusCode.Should().Be(HttpStatusCode.OK);
        responseGetCreatedContent.EnsureCreatedFromRequest(request);
        responseGetCreatedContent.Categories
            .Should()
            .BeEquivalentTo(productCategoryNames);
    }
}
