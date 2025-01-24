using Domain.OrderAggregate.Enumerations;

using IntegrationTests.Common;
using IntegrationTests.TestUtils.Extensions.HttpClient;
using IntegrationTests.TestUtils.Extensions.Orders;
using IntegrationTests.TestUtils.Seeds;

using SharedKernel.Models;

using Contracts.Orders;

using System.Net.Http.Json;
using FluentAssertions;
using Xunit.Abstractions;

namespace IntegrationTests.Orders;

/// <summary>
/// Integration tests for the process of getting customer orders.
/// </summary>
public class GetCustomerOrdersTests : BaseIntegrationTest
{
    /// <summary>
    /// Initiates a new instance of the <see cref="GetCustomerOrdersTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public GetCustomerOrdersTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output) : base(factory, output)
    {
    }

    /// <summary>
    /// Verifies that accessing the endpoint to retrieve a customer's orders without authentication returns Unauthorized.
    /// </summary>
    [Fact]
    public async Task GetCustomerOrders_WithoutAuthentication_ReturnsUnauthorized()
    {
        var response = await Client.GetAsync("/users/1/orders");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Ensures that a customer cannot retrieve another customer's orders, returning Forbidden.
    /// </summary>
    [Fact]
    public async Task GetCustomerOrders_WithoutSelfCustomer_ReturnsForbidden()
    {
        var customerWithOrdersType = SeedAvailableUsers.Customer;
        var otherCustomerType = SeedAvailableUsers.CustomerWithAddress;

        var customerWithOrders = UserSeed.GetSeedUser(customerWithOrdersType);

        await Client.LoginAs(otherCustomerType);
        var response = await Client.GetAsync($"/users/{customerWithOrders.Id}/orders");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Validates that an admin user or the customer themselves can retrieve the list of orders returning an OK response.
    /// </summary>
    /// <param name="userWithPermission">The user with either self or admin permissions.</param>
    [Theory]
    [InlineData(SeedAvailableUsers.Admin)]
    [InlineData(SeedAvailableUsers.Customer)]
    public async Task GetCustomerOrders_WithSelfOrAdminPermission_ReturnsTheOrders(
        SeedAvailableUsers userWithPermission
    )
    {
        var customerWithOrdersType = SeedAvailableUsers.Customer;

        var customerWithOrders = UserSeed.GetSeedUser(customerWithOrdersType);
        var expectedCustomerOrders = OrderSeed.GetUserOrders(customerWithOrders.Id);

        await Client.LoginAs(userWithPermission);
        var response = await Client.GetAsync($"/users/{customerWithOrders.Id}/orders");
        var responseContent = await response.Content.ReadFromJsonAsync<IEnumerable<OrderResponse>>();

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        responseContent.EnsureCorrespondsTo(expectedCustomerOrders);
    }

    /// <summary>
    /// Tests that a user with proper permissions can filter customer orders by their status 
    /// by passing a status query parameter, returning an OK response.
    /// </summary>
    /// <param name="status">The order status to filter by.</param>
    [Theory]
    [InlineData(nameof(OrderStatus.Canceled))]
    [InlineData(nameof(OrderStatus.Pending))]
    [InlineData(nameof(OrderStatus.Paid))]
    public async Task GetCustomerOrders_WithPermissionAndStatusFilter_ReturnsTheFilteredOrders(string status)
    {
        var statusId = BaseEnumeration.FromDisplayName<OrderStatus>(status).Id;
        var customerWithOrdersType = SeedAvailableUsers.Customer;
        var customerWithOrders = UserSeed.GetSeedUser(customerWithOrdersType);
        var expectedFilteredOrders = OrderSeed
            .GetUserOrders(customerWithOrders.Id)
            .Where(order => order.OrderStatusId == statusId)
            .ToList();

        await Client.LoginAs(customerWithOrdersType);
        var response = await Client.GetAsync($"/users/{customerWithOrders.Id}/orders?status={status}");
        var responseContent = await response.Content.ReadFromJsonAsync<IEnumerable<OrderResponse>>();

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        responseContent.EnsureCorrespondsTo(expectedFilteredOrders);
    }
}
