using Domain.ProductAggregate;

using Contracts.Products;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Products;
using IntegrationTests.Common.Seeds.Abstracts;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.TestUtils.Extensions.Http;
using IntegrationTests.Products.TestUtils;

using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit.Abstractions;

namespace IntegrationTests.Products;

/// <summary>
/// Integration tests for the process of increment a product's inventory quantity available.
/// </summary>
public class UpdateProductInventoryTests : BaseIntegrationTest
{
    private readonly IDataSeed<ProductSeedType, Product> _seedProduct;

    /// <summary>
    /// Initiates a new instance of the <see cref="UpdateProductInventoryTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public UpdateProductInventoryTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output) : base(factory, output)
    {
        _seedProduct = SeedManager.GetSeed<ProductSeedType, Product>();
    }

    /// <summary>
    /// Tests when the user is not authenticated the response is unauthorized.
    /// </summary>
    [Fact]
    public async Task UpdateProductInventory_WhenUserIsNotAuthenticated_ReturnsUnauthorized()
    {
        var request = UpdateProductInventoryRequestUtils.CreateRequest();

        var response = await RequestService.Client.PutAsJsonAsync("/products/1/inventory", request);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Tests that when a user that is not admin tries to update a product's inventory the response is forbidden.
    /// </summary>
    /// <param name="customerUserType">The customer type to be authenticated.</param>
    [Theory]
    [InlineData(UserSeedType.CUSTOMER_WITH_ADDRESS)]
    [InlineData(UserSeedType.CUSTOMER)]
    public async Task UpdateProductInventory_WhenUserIsNotAdmin_ReturnsForbidden(UserSeedType customerUserType)
    {
        var request = UpdateProductInventoryRequestUtils.CreateRequest();

        await RequestService.LoginAsAsync(customerUserType);
        var response = await RequestService.Client.PutAsJsonAsync("/products/1/inventory", request);

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

        await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var response = await RequestService.Client.PutAsJsonAsync($"/products/{notFoundId}/inventory", request);
        var responseContent = await response.Content.ReadRequiredFromJsonAsync<ProblemDetails>();

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        responseContent.Status.Should().Be((int)HttpStatusCode.NotFound);
        responseContent.Title.Should().Be("Product Not Found");
        responseContent.Detail.Should().Be($"It was not possible to increment the inventory of the product with id {notFoundId} because the product does not exist");
    }

    /// <summary>
    /// Tests when the user is admin and the quantity to add in the inventory is valid the product inventory is updated and the response is no content.
    /// Also fetches the product by id to test if the quantity in inventory was indeed updated.
    /// </summary>
    /// <param name="productType">The product type to update the inventory.</param>
    /// <param name="quantityToIncrement">The quantity to add to the inventory.</param>
    [Theory]
    [InlineData(ProductSeedType.PENCIL, 20)]
    [InlineData(ProductSeedType.COMPUTER_ON_SALE, 5)]
    [InlineData(ProductSeedType.CHAIN_BRACELET, 900)]
    public async Task UpdateProductInventory_WhenUserIsAdminAndQuantityToAddIsValid_UpdatesInventoryAndReturnsNoContent(
        ProductSeedType productType,
        int quantityToIncrement
    )
    {
        var product = _seedProduct.GetByType(productType);
        var initialQuantity = product.Inventory.QuantityAvailable;
        var request = UpdateProductInventoryRequestUtils.CreateRequest(quantityToIncrement: quantityToIncrement);
        var expectedQuantityAfterUpdate = initialQuantity + quantityToIncrement;

        await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var putResponse = await RequestService.Client.PutAsJsonAsync($"/products/{product.Id}/inventory", request);
        var getResponse = await RequestService.Client.GetAsync($"/products/{product.Id}");
        var getResponseContent = await getResponse.Content.ReadRequiredFromJsonAsync<ProductResponse>();

        putResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        getResponseContent.QuantityAvailable.Should().Be(expectedQuantityAfterUpdate);
    }
}
