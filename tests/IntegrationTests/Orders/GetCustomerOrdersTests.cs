using Domain.OrderAggregate;
using Domain.UserAggregate.ValueObjects;
using Domain.UserAggregate;
using Domain.OrderAggregate.Enumerations;

using SharedKernel.Models;

using Contracts.Orders;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Abstracts;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.Common.Seeds.Orders;
using IntegrationTests.TestUtils.Extensions.Http;
using IntegrationTests.TestUtils.Extensions.Orders;

using FluentAssertions;
using Xunit.Abstractions;

namespace IntegrationTests.Orders;

/// <summary>
/// Integration tests for the process of getting customer orders.
/// </summary>
public class GetCustomerOrdersTests : BaseIntegrationTest
{
    private readonly IDataSeed<UserSeedType, User> _seedUser;
    private readonly IDataSeed<OrderSeedType, Order> _seedOrder;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetCustomerOrdersTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public GetCustomerOrdersTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output) : base(factory, output)
    {
        _seedUser = SeedManager.GetSeed<UserSeedType, User>();
        _seedOrder = SeedManager.GetSeed<OrderSeedType, Order>();
    }

    /// <summary>
    /// Verifies that accessing the endpoint to retrieve a customer's orders without authentication returns Unauthorized.
    /// </summary>
    [Fact]
    public async Task GetCustomerOrders_WithoutAuthentication_ReturnsUnauthorized()
    {
        var response = await RequestService.Client.GetAsync("/users/1/orders");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Ensures that a customer cannot retrieve another customer's orders, returning Forbidden.
    /// </summary>
    [Fact]
    public async Task GetCustomerOrders_WithoutSelfCustomer_ReturnsForbidden()
    {
        var customerWithOrdersType = UserSeedType.CUSTOMER;
        var otherCustomerType = UserSeedType.CUSTOMER_WITH_ADDRESS;

        var customerWithOrders = _seedUser.GetByType(customerWithOrdersType);

        await RequestService.LoginAsAsync(otherCustomerType);
        var response = await RequestService.Client.GetAsync($"/users/{customerWithOrders.Id}/orders");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Validates that an admin user or the customer themselves can retrieve the list of orders returning an OK response.
    /// </summary>
    /// <param name="userWithPermission">The user with either self or admin permissions.</param>
    [Theory]
    [InlineData(UserSeedType.ADMIN)]
    [InlineData(UserSeedType.CUSTOMER)]
    public async Task GetCustomerOrders_WithSelfOrAdminPermission_ReturnsTheOrders(
        UserSeedType userWithPermission
    )
    {
        var customerWithOrdersType = UserSeedType.CUSTOMER;

        var customerWithOrders = _seedUser.GetByType(customerWithOrdersType);
        var expectedCustomerOrders = GetUserOrders(customerWithOrders.Id);

        await RequestService.LoginAsAsync(userWithPermission);
        var response = await RequestService.Client.GetAsync($"/users/{customerWithOrders.Id}/orders");
        var responseContent = await response.Content.ReadRequiredFromJsonAsync<IEnumerable<OrderResponse>>();

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        responseContent.EnsureCorrespondsTo(expectedCustomerOrders);
    }

    /// <summary>
    /// Tests that a user with proper permissions can filter customer orders by their status 
    /// by passing a status query parameter, returning an OK response.
    /// </summary>
    /// <param name="statusName">The order status name to filter by.</param>
    [Theory]
    [InlineData(nameof(OrderStatus.Canceled))]
    [InlineData(nameof(OrderStatus.Pending))]
    [InlineData(nameof(OrderStatus.Paid))]
    public async Task GetCustomerOrders_WithPermissionAndStatusFilter_ReturnsTheFilteredOrders(string statusName)
    {
        var status = BaseEnumeration.FromDisplayName<OrderStatus>(statusName);
        var customerWithOrdersType = UserSeedType.CUSTOMER;
        var customerWithOrders = _seedUser.GetByType(customerWithOrdersType);
        var expectedFilteredOrders = GetUserOrderByStatus(customerWithOrders.Id, status);

        await RequestService.LoginAsAsync(customerWithOrdersType);
        var response = await RequestService.Client.GetAsync($"/users/{customerWithOrders.Id}/orders?status={statusName}");
        var responseContent = await response.Content.ReadRequiredFromJsonAsync<IEnumerable<OrderResponse>>();

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        responseContent.EnsureCorrespondsTo(expectedFilteredOrders);
    }

    private IReadOnlyList<Order> GetUserOrders(UserId ownerId)
    {
        return _seedOrder
            .ListAll(o => o.OwnerId == ownerId);
    }

    private IReadOnlyList<Order> GetUserOrderByStatus(UserId ownerId, OrderStatus status)
    {
        return _seedOrder
            .ListAll(o => o.OwnerId == ownerId && o.OrderStatusId == status.Id);
    }
}
