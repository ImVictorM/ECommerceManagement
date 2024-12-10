using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using IntegrationTests.Common;
using IntegrationTests.Orders.TestUtils;
using IntegrationTests.TestUtils.Extensions.HttpClient;
using IntegrationTests.TestUtils.Seeds;
using Microsoft.AspNetCore.Mvc;
using WebApi.Endpoints;
using Xunit.Abstractions;

namespace IntegrationTests.Orders;

/// <summary>
/// Integration tests for the process of placing an order.
/// </summary>
public class PlaceOrderTests : BaseIntegrationTest
{
    /// <summary>
    /// Initiates a new instance of the <see cref="PlaceOrderTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public PlaceOrderTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output) : base(factory, output)
    {
    }

    /// <summary>
    /// Tests when the user trying to place an order is not authenticated it is returned a unauthorized response.
    /// </summary>
    [Fact]
    public async Task PlaceOrder_WhenUserIsNotAuthenticated_ReturnsUnauthorized()
    {
        var request = PlaceOrderRequestUtils.CreateRequest();

        var response = await Client.PostAsJsonAsync(OrderEndpoints.BaseEndpoint, request);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Tests that only authenticated customers can place orders.
    /// </summary>
    /// <param name="userWithoutCustomerRoleType">The user without the customer role.</param>
    [Theory]
    [InlineData(SeedAvailableUsers.Admin)]
    [InlineData(SeedAvailableUsers.OtherAdmin)]
    public async Task PlaceOrder_WhenUserDoesNotHaveCustomerRoles_ReturnsForbidden(SeedAvailableUsers userWithoutCustomerRoleType)
    {
        var request = PlaceOrderRequestUtils.CreateRequest();

        await Client.LoginAs(userWithoutCustomerRoleType);
        var response = await Client.PostAsJsonAsync(OrderEndpoints.BaseEndpoint, request);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Tests customers can place orders and the order is created correctly.
    /// </summary>
    /// <param name="userWithCustomerRoleType">The customer.</param>
    [Theory]
    [InlineData(SeedAvailableUsers.Customer)]
    [InlineData(SeedAvailableUsers.CustomerWithAddress)]
    public async Task PlaceOrder_WhenParametersAreCorrectAndUserIsAuthorized_CreatesOrderAndReturnsCreated(SeedAvailableUsers userWithCustomerRoleType)
    {
        var request = PlaceOrderRequestUtils.CreateRequest(
            products: [
                PlaceOrderRequestUtils.CreateOrderProduct(SeedAvailableProducts.PENCIL, 2),
                PlaceOrderRequestUtils.CreateOrderProduct(SeedAvailableProducts.COMPUTER_WITH_DISCOUNTS, 1)
            ]
        );

        await Client.LoginAs(userWithCustomerRoleType);
        var response = await Client.PostAsJsonAsync(OrderEndpoints.BaseEndpoint, request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    /// <summary>
    /// Tests when some of the products does not have available items in inventory the return is a bad request.
    /// </summary>
    [Fact]
    public async Task PlaceOrder_WhenSomeOfTheProductsDoesNotHaveAvailableItemsInInventory_ReturnsBadRequest()
    {
        var pencil = ProductSeed.GetSeedProduct(SeedAvailableProducts.PENCIL);
        var quantityToBuy = pencil.Inventory.QuantityAvailable + 1;

        var request = PlaceOrderRequestUtils.CreateRequest(
            products: [
                PlaceOrderRequestUtils.CreateOrderProduct(SeedAvailableProducts.PENCIL, quantityToBuy),
                PlaceOrderRequestUtils.CreateOrderProduct(SeedAvailableProducts.COMPUTER_WITH_DISCOUNTS, 1)
            ]
        );

        await Client.LoginAs(SeedAvailableUsers.Customer);
        var response = await Client.PostAsJsonAsync(OrderEndpoints.BaseEndpoint, request);
        var responseContent = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        responseContent!.Status.Should().Be((int)HttpStatusCode.BadRequest);
        responseContent.Detail.Should().Be($"{pencil.Name} is not available. Current inventory: {pencil.Inventory.QuantityAvailable}. Order quantity: {quantityToBuy}");
        responseContent.Title.Should().Be("Product Not Available");
    }
}
