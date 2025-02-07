using Contracts.Orders;

using Domain.OrderAggregate;
using Domain.OrderAggregate.Enumerations;

using SharedKernel.Models;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.Common.Seeds.Abstracts;
using IntegrationTests.Common.Seeds.Orders;
using IntegrationTests.TestUtils.Extensions.Orders;
using IntegrationTests.TestUtils.Extensions.Http;
using IntegrationTests.TestUtils.Constants;

using FluentAssertions;
using Xunit.Abstractions;

namespace IntegrationTests.Orders;

/// <summary>
/// Integration tests for the process of getting orders.
/// </summary>
public class GetOrdersTests : BaseIntegrationTest
{
    private readonly IDataSeed<OrderSeedType, Order> _seedOrder;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetOrdersTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public GetOrdersTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output) : base(factory, output)
    {
        _seedOrder = SeedManager.GetSeed<OrderSeedType, Order>();
    }

    /// <summary>
    /// Tests that accessing the orders endpoint without authentication returns Unauthorized.
    /// </summary>
    [Fact]
    public async Task GetOrders_WithoutAuthentication_ReturnsUnauthorized()
    {
        var endpoint = TestConstants.OrderEndpoints.GetOrders;
        var response = await RequestService.Client.GetAsync(endpoint);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Tests that accessing the orders endpoint without an admin role returns Forbidden.
    /// </summary>
    [Fact]
    public async Task GetOrders_WithoutAdminRole_ReturnsForbidden()
    {
        var endpoint = TestConstants.OrderEndpoints.GetOrders;

        await RequestService.LoginAsAsync(UserSeedType.CUSTOMER);
        var response = await RequestService.Client.GetAsync(endpoint);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Tests that accessing the orders endpoint with an admin role returns the list of all orders with a OK response.
    /// </summary>
    [Fact]
    public async Task GetOrders_WithAdminRole_ReturnOrders()
    {
        var expectedReturnedOrders = _seedOrder.ListAll();
        var endpoint = TestConstants.OrderEndpoints.GetOrders;

        await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var response = await RequestService.Client.GetAsync(endpoint);

        var responseContent = await response.Content.ReadRequiredFromJsonAsync<IEnumerable<OrderResponse>>();

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        responseContent.EnsureCorrespondsTo(expectedReturnedOrders);
    }

    /// <summary>
    /// Tests that accessing the orders endpoint with an admin role and a valid status filter returns the filtered list of orders with a OK response.
    /// </summary>
    /// <param name="statusName">The order status to filter by.</param>
    [Theory]
    [InlineData(nameof(OrderStatus.Canceled))]
    [InlineData(nameof(OrderStatus.Pending))]
    [InlineData(nameof(OrderStatus.Paid))]
    public async Task GetOrders_WithAdminRoleAndStatusFilter_ReturnFilteredOrders(string statusName)
    {
        var status = BaseEnumeration.FromDisplayName<OrderStatus>(statusName);
        var expectedOrders = GetOrdersByStatus(status);
        var endpoint = TestConstants.OrderEndpoints.GetOrders;

        await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var response = await RequestService.Client.GetAsync($"{endpoint}?status={statusName}");
        var responseContent = await response.Content.ReadRequiredFromJsonAsync<IEnumerable<OrderResponse>>();

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        responseContent.EnsureCorrespondsTo(expectedOrders);
    }

    private IReadOnlyList<Order> GetOrdersByStatus(OrderStatus status)
    {
        return _seedOrder
            .ListAll(o => o.OrderStatus == status);
    }
}
