using Domain.UserAggregate;
using Domain.OrderAggregate;
using Domain.UserAggregate.ValueObjects;

using Contracts.Orders;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.Common.Seeds.Orders;
using IntegrationTests.Common.Seeds.Abstracts;
using IntegrationTests.TestUtils.Extensions.Orders;
using IntegrationTests.TestUtils.Extensions.Http;

using WebApi.Orders;

using FluentAssertions;
using Xunit.Abstractions;
using Microsoft.AspNetCore.Routing;

namespace IntegrationTests.Orders;

/// <summary>
/// Integration tests for the get customer order by id feature.
/// </summary>
public class GetCustomerOrderByIdTests : BaseIntegrationTest
{
    private readonly IDataSeed<UserSeedType, User> _seedUser;
    private readonly IDataSeed<OrderSeedType, Order> _seedOrder;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetCustomerOrderByIdTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public GetCustomerOrderByIdTests(
        IntegrationTestWebAppFactory
        factory, ITestOutputHelper output
    )
        : base(factory, output)
    {
        _seedUser = SeedManager.GetSeed<UserSeedType, User>();
        _seedOrder = SeedManager.GetSeed<OrderSeedType, Order>();
    }

    /// <summary>
    /// Verifies that accessing the endpoint to retrieve a customer's orders
    /// without authentication returns Unauthorized.
    /// </summary>
    [Fact]
    public async Task GetCustomerOrderById_WithoutAuthentication_ReturnsUnauthorized()
    {
        var endpoint = LinkGenerator.GetPathByName(
            nameof(CustomerOrderEndpoints.GetCustomerOrderById),
            new
            {
                userId = "1",
                orderId = "2"
            }
        );

        var response = await RequestService.Client.GetAsync(endpoint);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Ensures that a customer cannot retrieve another customer's order, returning Forbidden.
    /// </summary>
    [Fact]
    public async Task GetCustomerOrderById_WithoutSelfCustomer_ReturnsForbidden()
    {
        var customerWithOrdersType = UserSeedType.CUSTOMER;
        var otherCustomerType = UserSeedType.CUSTOMER_WITH_ADDRESS;
        var customerWithOrders = _seedUser.GetByType(customerWithOrdersType);
        var customerOrder = GetCustomerFistOrder(customerWithOrders.Id);

        var endpoint = LinkGenerator.GetPathByName(
            nameof(CustomerOrderEndpoints.GetCustomerOrderById),
            new
            {
                userId = customerWithOrders.Id.ToString(),
                orderId = customerOrder.Id.ToString()
            }
        );

        await RequestService.LoginAsAsync(otherCustomerType);
        var response = await RequestService.Client.GetAsync(endpoint);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Verifies when the order does not exists it is returned a not found response.
    /// </summary>
    [Fact]
    public async Task GetCustomerOrderById_WithNonexistingOrderId_ReturnsNotFound()
    {
        var nonexistingOrderId = "404";

        var endpoint = LinkGenerator.GetPathByName(
            nameof(CustomerOrderEndpoints.GetCustomerOrderById),
            new
            {
                userId = "1",
                orderId = nonexistingOrderId
            }
        );

        await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var response = await RequestService.Client.GetAsync(endpoint);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    /// <summary>
    /// Verifies an admin user or the customer themselves
    /// can retrieve the order returning an OK response.
    /// </summary>
    /// <param name="userWithPermission">
    /// The user with either self or admin permissions.
    /// </param>
    [Theory]
    [InlineData(UserSeedType.ADMIN)]
    [InlineData(UserSeedType.CUSTOMER)]
    public async Task GetCustomerOrderById_WithSelfOrAdminPermission_ReturnsTheOrder(
        UserSeedType userWithPermission
    )
    {
        var orderOwnerType = UserSeedType.CUSTOMER;
        var orderOwner = _seedUser.GetByType(orderOwnerType);
        var customerOrder = GetCustomerFistOrder(orderOwner.Id);

        var endpoint = LinkGenerator.GetPathByName(
            nameof(CustomerOrderEndpoints.GetCustomerOrderById),
            new
            {
                userId = orderOwner.Id.ToString(),
                orderId = customerOrder.Id.ToString()
            }
        );

        await RequestService.LoginAsAsync(userWithPermission);
        var response = await RequestService.Client.GetAsync(endpoint);
        var responseContent = await response.Content
            .ReadRequiredFromJsonAsync<OrderDetailedResponse>();

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        responseContent.EnsureCorrespondsTo(customerOrder);
    }

    private Order GetCustomerFistOrder(UserId ownerId)
    {
        return _seedOrder
            .ListAll(o => o.OwnerId == ownerId)[0];
    }
}
