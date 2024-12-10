using System.Net;
using System.Net.Http.Json;
using Contracts.Products;
using Domain.ProductAggregate;
using FluentAssertions;
using IntegrationTests.Common;
using IntegrationTests.Products.TestUtils;
using IntegrationTests.TestUtils.Extensions.HttpClient;
using IntegrationTests.TestUtils.Seeds;
using Microsoft.AspNetCore.Mvc;
using Xunit.Abstractions;

namespace IntegrationTests.Products;

/// <summary>
/// Integration tests for the process of increment a product's inventory quantity available.
/// </summary>
public class UpdateProductInventoryTests : BaseIntegrationTest
{
    /// <summary>
    /// Initiates a new instance of the <see cref="UpdateProductInventoryTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public UpdateProductInventoryTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output) : base(factory, output)
    {
    }

    /// <summary>
    /// Provides pairs of products and a random quantity to add to the product inventory.
    /// </summary>
    public static IEnumerable<object[]> ProductAndQuantityToIncrementPairs()
    {
        foreach (var product in ProductSeed.ListProducts(p => p.IsActive))
        {
            yield return new object[] { product, new Random().Next(1, 51) };
        }
    }

    /// <summary>
    /// Tests when the user is not authenticated the response is unauthorized.
    /// </summary>
    [Fact]
    public async Task UpdateProductInventory_WhenUserIsNotAuthenticated_ReturnsUnauthorized()
    {
        var request = UpdateProductInventoryRequestUtils.CreateRequest();

        var response = await Client.PutAsJsonAsync("/products/1/inventory", request);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Tests that when a user that is not admin tries to update a product's inventory the response is forbidden.
    /// </summary>
    /// <param name="customerUserType">The customer type to be authenticated.</param>
    [Theory]
    [InlineData(SeedAvailableUsers.CustomerWithAddress)]
    [InlineData(SeedAvailableUsers.Customer)]
    public async Task UpdateProductInventory_WhenUserIsNotAdmin_ReturnsForbidden(SeedAvailableUsers customerUserType)
    {
        var request = UpdateProductInventoryRequestUtils.CreateRequest();

        await Client.LoginAs(customerUserType);
        var response = await Client.PutAsJsonAsync("/products/1/inventory", request);

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
        await Client.LoginAs(SeedAvailableUsers.Admin);

        var response = await Client.PutAsJsonAsync($"/products/{notFoundId}/inventory", request);
        var responseContent = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        responseContent!.Status.Should().Be((int)HttpStatusCode.NotFound);
        responseContent.Title.Should().Be("Product Not Found");
        responseContent.Detail.Should().Be($"It was not possible to increment the inventory of the product with id {notFoundId} because the product does not exist");
    }

    /// <summary>
    /// Tests when the user is admin and the quantity to add in the inventory is valid the product inventory is updated and the response is no content.
    /// Also fetches the product by id to test if the quantity in inventory was indeed updated.
    /// </summary>
    /// <param name="product">The product to update the inventory.</param>
    /// <param name="quantityToIncrement">The quantity to add to the inventory.</param>
    [Theory]
    [MemberData(nameof(ProductAndQuantityToIncrementPairs))]
    public async Task UpdateProductInventory_WhenUserIsAdminAndQuantityToAddIsValid_UpdatesInventoryAndReturnsNoContent(
        Product product,
        int quantityToIncrement
    )
    {
        var initialQuantity = product.Inventory.QuantityAvailable;
        var request = UpdateProductInventoryRequestUtils.CreateRequest(quantityToIncrement: quantityToIncrement);
        var expectedQuantityAfterUpdate = initialQuantity + quantityToIncrement;

        await Client.LoginAs(SeedAvailableUsers.Admin);
        var putResponse = await Client.PutAsJsonAsync($"/products/{product.Id}/inventory", request);
        var getResponse = await Client.GetAsync($"/products/{product.Id}");
        var getResponseContent = await getResponse.Content.ReadFromJsonAsync<ProductResponse>();

        putResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        getResponseContent!.QuantityAvailable.Should().Be(expectedQuantityAfterUpdate);
    }
}
