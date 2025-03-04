using Contracts.Orders;

using Domain.OrderAggregate;
using Domain.OrderAggregate.Enumerations;

using SharedKernel.Models;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.Common.Seeds.Orders;
using IntegrationTests.TestUtils.Extensions.Orders;
using IntegrationTests.TestUtils.Extensions.Http;

using WebApi.Orders;

using FluentAssertions;
using Xunit.Abstractions;
using Microsoft.AspNetCore.Routing;
using System.Net;

namespace IntegrationTests.Orders;

/// <summary>
/// Integration tests for the get orders feature.
/// </summary>
public class GetOrdersTests : BaseIntegrationTest
{
    private readonly IOrderSeed _seedOrder;
    private readonly string? _endpoint;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetOrdersTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public GetOrdersTests(
        IntegrationTestWebAppFactory factory,
        ITestOutputHelper output
    ) : base(factory, output)
    {
        _seedOrder = SeedManager.GetSeed<IOrderSeed>();

        _endpoint = LinkGenerator.GetPathByName(
            nameof(OrderEndpoints.GetOrders)
        );
    }

    /// <summary>
    /// Verifies that accessing the orders endpoint without
    /// authentication returns Unauthorized.
    /// </summary>
    [Fact]
    public async Task GetOrders_WithoutAuthentication_ReturnsUnauthorized()
    {
        var response = await RequestService.CreateClient().GetAsync(_endpoint);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Verifies that accessing the orders endpoint without the admin
    /// role returns Forbidden.
    /// </summary>
    [Fact]
    public async Task GetOrders_WithoutAdminRole_ReturnsForbidden()
    {
        var client = await RequestService.LoginAsAsync(UserSeedType.CUSTOMER);

        var response = await client.GetAsync(_endpoint);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Verifies that accessing the orders endpoint with the admin role
    /// returns the list of all orders with an OK response.
    /// </summary>
    [Fact]
    public async Task GetOrders_WithAdminRole_ReturnOk()
    {
        var expectedReturnedOrders = _seedOrder.ListAll();

        var client = await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var response = await client.GetAsync(_endpoint);

        var responseContent = await response.Content
            .ReadRequiredFromJsonAsync<IEnumerable<OrderResponse>>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent.EnsureCorrespondsTo(expectedReturnedOrders);
    }

    /// <summary>
    /// Tests that accessing the orders endpoint with the admin role and a valid
    /// status filter returns an OK response containing the filtered orders.
    /// </summary>
    /// <param name="statusName">The order status to filter by.</param>
    [Theory]
    [InlineData(nameof(OrderStatus.Canceled))]
    [InlineData(nameof(OrderStatus.Pending))]
    [InlineData(nameof(OrderStatus.Paid))]
    public async Task GetOrders_WithAdminRoleAndStatusFilter_ReturnsOk(
        string statusName
    )
    {
        var status = BaseEnumeration.FromDisplayName<OrderStatus>(statusName);
        var expectedOrders = GetOrdersByStatus(status);

        var client = await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var response = await client.GetAsync($"{_endpoint}?status={statusName}");

        var responseContent = await response.Content
            .ReadRequiredFromJsonAsync<IEnumerable<OrderResponse>>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent.EnsureCorrespondsTo(expectedOrders);
    }

    private IReadOnlyList<Order> GetOrdersByStatus(OrderStatus status)
    {
        return _seedOrder.ListAll(o => o.OrderStatus == status);
    }
}
