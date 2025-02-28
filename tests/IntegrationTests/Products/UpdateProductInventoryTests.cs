using Domain.ProductAggregate;

using Contracts.Products;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Products;
using IntegrationTests.Common.Seeds.Abstracts;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.TestUtils.Extensions.Http;
using IntegrationTests.Products.TestUtils;

using WebApi.Products;

using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Xunit.Abstractions;

namespace IntegrationTests.Products;

/// <summary>
/// Integration tests for the update product inventory feature.
/// </summary>
public class UpdateProductInventoryTests : BaseIntegrationTest
{
    private readonly IDataSeed<ProductSeedType, Product> _seedProduct;

    /// <summary>
    /// Initiates a new instance of the <see cref="UpdateProductInventoryTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public UpdateProductInventoryTests(
        IntegrationTestWebAppFactory factory,
        ITestOutputHelper output
    ) : base(factory, output)
    {
        _seedProduct = SeedManager.GetSeed<ProductSeedType, Product>();
    }

    /// <summary>
    /// Verifies when the user is not authenticated the response is unauthorized.
    /// </summary>
    [Fact]
    public async Task UpdateProductInventory_WithoutAuthentication_ReturnsUnauthorized()
    {
        var request = UpdateProductInventoryRequestUtils.CreateRequest();

        var endpoint = LinkGenerator.GetPathByName(
            nameof(ProductEndpoints.UpdateProductInventory),
            new { id = "1" }
        );

        var response = await RequestService.CreateClient().PutAsJsonAsync(
            endpoint,
            request
        );

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Verifies when a user that is not admin tries to update a product's
    /// inventory the response is forbidden.
    /// </summary>
    /// <param name="customerUserType">
    /// The customer type to be authenticated.
    /// </param>
    [Theory]
    [InlineData(UserSeedType.CUSTOMER_WITH_ADDRESS)]
    [InlineData(UserSeedType.CUSTOMER)]
    public async Task UpdateProductInventory_WhenUserIsNotAdmin_ReturnsForbidden(
        UserSeedType customerUserType
    )
    {
        var request = UpdateProductInventoryRequestUtils.CreateRequest();

        var endpoint = LinkGenerator.GetPathByName(
            nameof(ProductEndpoints.UpdateProductInventory),
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
    /// Tests that when the product does not exist the response is not found.
    /// </summary>
    [Fact]
    public async Task UpdateProductInventory_WhenProductDoesNotExist_ReturnsNotFound()
    {
        var notFoundId = "404";
        var request = UpdateProductInventoryRequestUtils.CreateRequest();
        var endpoint = LinkGenerator.GetPathByName(
            nameof(ProductEndpoints.UpdateProductInventory),
            new { id = notFoundId }
        );

        var client = await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var response = await client.PutAsJsonAsync(
            endpoint,
            request
        );
        var responseContent = await response.Content
            .ReadRequiredFromJsonAsync<ProblemDetails>();

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        responseContent.Status.Should().Be((int)HttpStatusCode.NotFound);
        responseContent.Title.Should().Be("Product Not Found");
        responseContent.Detail.Should().Be(
            $"It was not possible to increment the inventory of the product" +
            $" with id {notFoundId} because the product does not exist"
        );
    }

    /// <summary>
    /// Verifies when the user is admin and the quantity to add in the inventory
    /// is valid the product inventory is updated and the response is no content.
    /// Also fetches the product by id to test if the quantity in inventory
    /// was indeed updated.
    /// </summary>
    /// <param name="productType">
    /// The product type to update the inventory.
    /// </param>
    /// <param name="quantityToIncrement">
    /// The quantity to add to the inventory.
    /// </param>
    [Theory]
    [InlineData(ProductSeedType.PENCIL, 20)]
    [InlineData(ProductSeedType.COMPUTER_ON_SALE, 5)]
    [InlineData(ProductSeedType.CHAIN_BRACELET, 900)]
    public async Task UpdateProductInventory_WithAdminAuthenticationAndValidQuantity_ReturnsNoContent(
        ProductSeedType productType,
        int quantityToIncrement
    )
    {
        var product = _seedProduct.GetByType(productType);
        var initialQuantity = product.Inventory.QuantityAvailable;
        var request = UpdateProductInventoryRequestUtils.CreateRequest(
            quantityToIncrement: quantityToIncrement
        );
        var expectedQuantityAfterUpdate = initialQuantity + quantityToIncrement;

        var endpointUpdateProductInventory = LinkGenerator.GetPathByName(
            nameof(ProductEndpoints.UpdateProductInventory),
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
            .Be(expectedQuantityAfterUpdate);
    }
}
