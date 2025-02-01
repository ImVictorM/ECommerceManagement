using Contracts.Orders;

using Domain.OrderAggregate.Enumerations;

using IntegrationTests.Common;
using IntegrationTests.TestUtils.Extensions.HttpClient;
using IntegrationTests.TestUtils.Seeds;
using IntegrationTests.TestUtils.Extensions.Orders;

using SharedKernel.Models;

using System.Net.Http.Json;
using FluentAssertions;
using Xunit.Abstractions;

namespace IntegrationTests.Orders;

/// <summary>
/// Integration tests for the process of getting orders.
/// </summary>
public class GetOrdersTests : BaseIntegrationTest
{
    /// <summary>
    /// Initiates a new instance of the <see cref="GetOrdersTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public GetOrdersTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output) : base(factory, output)
    {
    }

    /// <summary>
    /// Tests that accessing the orders endpoint without authentication returns Unauthorized.
    /// </summary>
    [Fact]
    public async Task GetOrders_WithoutAuthentication_ReturnsUnauthorized()
    {
        var response = await Client.GetAsync("/orders");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Tests that accessing the orders endpoint without an admin role returns Forbidden.
    /// </summary>
    [Fact]
    public async Task GetOrders_WithoutAdminRole_ReturnsForbidden()
    {
        await Client.LoginAs(SeedAvailableUsers.CUSTOMER);
        var response = await Client.GetAsync("/orders");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Tests that accessing the orders endpoint with an admin role returns the list of all orders with a OK response.
    /// </summary>
    [Fact]
    public async Task GetOrders_WithAdminRole_ReturnOrders()
    {
        var expectedReturnedOrders = OrderSeed.ListOrders();

        await Client.LoginAs(SeedAvailableUsers.ADMIN);
        var response = await Client.GetAsync("/orders");

        var responseContent = await response.Content.ReadFromJsonAsync<IEnumerable<OrderResponse>>();

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        responseContent.EnsureCorrespondsTo(expectedReturnedOrders);
    }

    /// <summary>
    /// Tests that accessing the orders endpoint with an admin role and a valid status filter returns the filtered list of orders with a OK response.
    /// </summary>
    /// <param name="status">The order status to filter by.</param>
    [Theory]
    [InlineData(nameof(OrderStatus.Canceled))]
    [InlineData(nameof(OrderStatus.Pending))]
    [InlineData(nameof(OrderStatus.Paid))]
    public async Task GetOrders_WithAdminRoleAndStatusFilter_ReturnFilteredOrders(string status)
    {
        var statusId = BaseEnumeration.FromDisplayName<OrderStatus>(status).Id;

        var expectedOrders = OrderSeed.ListOrders().Where(order => order.OrderStatusId == statusId).ToList();

        await Client.LoginAs(SeedAvailableUsers.ADMIN);
        var response = await Client.GetAsync($"/orders?status={status}");
        var responseContent = await response.Content.ReadFromJsonAsync<IEnumerable<OrderResponse>>();

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        responseContent.EnsureCorrespondsTo(expectedOrders);
    }
}
