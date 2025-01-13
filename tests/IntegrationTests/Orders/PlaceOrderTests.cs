using IntegrationTests.Common;
using IntegrationTests.Orders.TestUtils;
using IntegrationTests.TestUtils.Extensions.HttpClient;
using IntegrationTests.TestUtils.Seeds;

using WebApi.Endpoints;

using Contracts.Orders;

using SharedKernel.Services;

using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
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
    public async Task PlaceOrder_WhenUserDoesNotHaveCustomerRole_ReturnsForbidden(SeedAvailableUsers userWithoutCustomerRoleType)
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
        var pencil = ProductSeed.GetSeedProduct(SeedAvailableProducts.PENCIL);
        var computer = ProductSeed.GetSeedProduct(SeedAvailableProducts.COMPUTER_ON_SALE);
        var couponApplied = CouponSeed.GetSeedCoupon(SeedAvailableCoupons.TECH_COUPON);

        var request = PlaceOrderRequestUtils.CreateRequest(
            products:
            [
                new OrderProductRequest(pencil.Id.ToString(), 1),
                new OrderProductRequest(computer.Id.ToString(), 2),
            ],
            couponAppliedIds:
            [
                couponApplied.Id.ToString()
            ]
        );

        var expectedCreatedPencilResponse = new OrderProductResponse(
            pencil.Id.ToString(),
            1,
            pencil.BasePrice,
            SaleSeed.CalculateExpectedPriceAfterApplyingSales(pencil)
        );

        var expectedCreatedComputerResponse = new OrderProductResponse(
            computer.Id.ToString(),
            2,
            computer.BasePrice,
            SaleSeed.CalculateExpectedPriceAfterApplyingSales(computer)
        );

        var expectedComputerTotal = expectedCreatedComputerResponse.PurchasedPrice * expectedCreatedComputerResponse.Quantity;
        var expectedPencilTotal = expectedCreatedPencilResponse.PurchasedPrice * expectedCreatedPencilResponse.Quantity;

        var expectedTotalPrice = DiscountService.ApplyDiscounts(
            expectedComputerTotal + expectedPencilTotal,
            [couponApplied.Discount]
        );

        var orderOwner = await Client.LoginAs(userWithCustomerRoleType);

        Client.DefaultRequestHeaders.Add("X-Idempotency-Key", Guid.NewGuid().ToString());

        var createResponse = await Client.PostAsJsonAsync(OrderEndpoints.BaseEndpoint, request);
        var resourceLocation = createResponse.Headers.Location;
        var getResourceResponse = await Client.GetAsync(resourceLocation);
        var createdResourceContent = await getResourceResponse.Content.ReadFromJsonAsync<OrderDetailedResponse>();

        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        createdResourceContent!.Status.Should().Be("pending");
        createdResourceContent.Should().NotBeNull();
        createdResourceContent.Products.Should().Contain(expectedCreatedPencilResponse);
        createdResourceContent.Products.Should().Contain(expectedCreatedComputerResponse);
        createdResourceContent.OwnerId.Should().Be(orderOwner.Id.ToString());
        createdResourceContent.Total.Should().Be(expectedTotalPrice);
        createdResourceContent.Payment.Should().NotBeNull();
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
                new OrderProductRequest(pencil.Id.ToString(), quantityToBuy),
            ]
        );

        await Client.LoginAs(SeedAvailableUsers.Customer);
        Client.DefaultRequestHeaders.Add("X-Idempotency-Key", Guid.NewGuid().ToString());
        var response = await Client.PostAsJsonAsync(OrderEndpoints.BaseEndpoint, request);
        var responseContent = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        responseContent!.Status.Should().Be((int)HttpStatusCode.BadRequest);
        responseContent.Detail.Should().Be("The product does not have available stock to complete the operation");
        responseContent.Title.Should().Be("Inventory Insufficient");
    }
}
