using Contracts.Products;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Products;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.TestUtils.Extensions.Http;
using IntegrationTests.Products.TestUtils;

using WebApi.Products;

using Microsoft.AspNetCore.Routing;
using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Xunit.Abstractions;

namespace IntegrationTests.Products;

/// <summary>
/// Integration tests for the add stock feature.
/// </summary>
public class AddStockTests : BaseIntegrationTest
{
    private readonly IProductSeed _seedProduct;

    /// <summary>
    /// Initiates a new instance of the <see cref="AddStockTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public AddStockTests(
        IntegrationTestWebAppFactory factory,
        ITestOutputHelper output
    ) : base(factory, output)
    {
        _seedProduct = SeedManager.GetSeed<IProductSeed>();
    }

    /// <summary>
    /// Verifies an unauthorized response is returned when the user is
    /// not authenticated.
    /// </summary>
    [Fact]
    public async Task AddStock_WithoutAuthentication_ReturnsUnauthorized()
    {
        var request = AddStockRequestUtils.CreateRequest();

        var endpoint = LinkGenerator.GetPathByName(
            nameof(ProductEndpoints.AddStock),
            new { id = "1" }
        );

        var response = await RequestService.CreateClient().PutAsJsonAsync(
            endpoint,
            request
        );

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Verifies when a user that is not admin tries to add stock to a product's
    /// inventory the response is forbidden.
    /// </summary>
    /// <param name="customerUserType">
    /// The customer type to be authenticated.
    /// </param>
    [Theory]
    [InlineData(UserSeedType.CUSTOMER_WITH_ADDRESS)]
    [InlineData(UserSeedType.CUSTOMER)]
    public async Task AddStock_WithoutAdminAuthentication_ReturnsForbidden(
        UserSeedType customerUserType
    )
    {
        var request = AddStockRequestUtils.CreateRequest();

        var endpoint = LinkGenerator.GetPathByName(
            nameof(ProductEndpoints.AddStock),
            new { id = "1" }
        );

        var client = await RequestService.LoginAsAsync(customerUserType);
        var response = await client.PutAsJsonAsync(
            endpoint,
            request
        );

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Verifies a not found response is returned when the product does not exist.
    /// </summary>
    [Fact]
    public async Task AddStock_WhenProductDoesNotExist_ReturnsNotFound()
    {
        var notFoundId = "404";
        var request = AddStockRequestUtils.CreateRequest();
        var endpoint = LinkGenerator.GetPathByName(
            nameof(ProductEndpoints.AddStock),
            new { id = notFoundId }
        );

        var client = await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var response = await client.PutAsJsonAsync(
            endpoint,
            request
        );

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    /// <summary>
    /// Verifies when the user is admin and the quantity to add in the inventory
    /// is valid the product inventory is updated and the response is no content.
    /// Also fetches the product by its identifier to test if the quantity in
    /// inventory was indeed updated.
    /// </summary>
    /// <param name="productType">
    /// The product type to update the inventory.
    /// </param>
    /// <param name="quantityToAdd">
    /// The quantity to add.
    /// </param>
    [Theory]
    [InlineData(ProductSeedType.PENCIL, 20)]
    [InlineData(ProductSeedType.COMPUTER_ON_SALE, 5)]
    [InlineData(ProductSeedType.CHAIN_BRACELET, 900)]
    public async Task AddStock_WithAdminAuthenticationAndValidQuantity_ReturnsNoContent(
        ProductSeedType productType,
        int quantityToAdd
    )
    {
        var product = _seedProduct.GetEntity(productType);
        var initialQuantity = product.Inventory.QuantityAvailable;
        var request = AddStockRequestUtils.CreateRequest(
            quantityToAdd: quantityToAdd
        );
        var expectedQuantityAfterAdding = initialQuantity + quantityToAdd;

        var endpointUpdateProductInventory = LinkGenerator.GetPathByName(
            nameof(ProductEndpoints.AddStock),
            new { id = product.Id.ToString() }
        );

        var endpointGetProductById = LinkGenerator.GetPathByName(
            nameof(ProductEndpoints.GetProductById),
            new { id = product.Id.ToString() }
        );

        var client = await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var responseUpdate = await client.PutAsJsonAsync(
            endpointUpdateProductInventory,
            request
        );
        var responseGetUpdated = await client.GetAsync(endpointGetProductById);

        var responseGetUpdatedContent = await responseGetUpdated.Content
            .ReadRequiredFromJsonAsync<ProductResponse>();

        responseUpdate.StatusCode.Should().Be(HttpStatusCode.NoContent);
        responseGetUpdated.StatusCode.Should().Be(HttpStatusCode.OK);
        responseGetUpdatedContent.QuantityAvailable
            .Should()
            .Be(expectedQuantityAfterAdding);
    }
}
