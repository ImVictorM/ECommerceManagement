using Contracts.Orders;

using IntegrationTests.Common;
using IntegrationTests.TestUtils.Extensions.HttpClient;
using IntegrationTests.TestUtils.Seeds;
using IntegrationTests.TestUtils.Extensions.Orders;

using System.Net.Http.Json;
using FluentAssertions;
using Xunit.Abstractions;

namespace IntegrationTests.Orders;

/// <summary>
/// Integration tests for the process of getting a customer's order by id.
/// </summary>
public class GetCustomerOrderByIdTests : BaseIntegrationTest
{
    /// <summary>
    /// Initiates a new instance of the <see cref="GetCustomerOrderByIdTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public GetCustomerOrderByIdTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output)
        : base(factory, output)
    {
    }

    /// <summary>
    /// Verifies that accessing the endpoint to retrieve a customer's orders without authentication returns Unauthorized.
    /// </summary>
    [Fact]
    public async Task GetCustomerOrderById_WithoutAuthentication_ReturnsUnauthorized()
    {
        var response = await Client.GetAsync("/users/1/orders/1");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Ensures that a customer cannot retrieve another customer's order, returning Forbidden.
    /// </summary>
    [Fact]
    public async Task GetCustomerOrderById_WithoutSelfCustomer_ReturnsForbidden()
    {
        var customerWithOrdersType = SeedAvailableUsers.Customer;
        var customerWithOrders = UserSeed.GetSeedUser(customerWithOrdersType);
        var customerOrder = OrderSeed.GetUserOrders(customerWithOrders.Id).First();
        var otherCustomerType = SeedAvailableUsers.CustomerWithAddress;

        await Client.LoginAs(otherCustomerType);
        var response = await Client.GetAsync($"/users/{customerWithOrders.Id}/orders/{customerOrder.Id}");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Tests when the order does not exists it is returned a not found response.
    /// </summary>
    [Fact]
    public async Task GetCustomerOrderById_WhenOrderDoesNotExist_ReturnsNotFound()
    {
        var notFoundId = "404";

        await Client.LoginAs(SeedAvailableUsers.Admin);
        var response = await Client.GetAsync($"/users/1/{notFoundId}");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    /// <summary>
    /// Validates that an admin user or the customer themselves can retrieve the order returning an OK response.
    /// </summary>
    /// <param name="userWithPermission">The user with either self or admin permissions.</param>
    [Theory]
    [InlineData(SeedAvailableUsers.Admin)]
    [InlineData(SeedAvailableUsers.Customer)]
    public async Task GetCustomerOrderById_WithSelfOrAdminPermission_ReturnsTheOrder(
        SeedAvailableUsers userWithPermission
    )
    {
        var orderOwnerType = SeedAvailableUsers.Customer;
        var orderOwner = UserSeed.GetSeedUser(orderOwnerType);
        var customerOrder = OrderSeed.GetUserOrders(orderOwner.Id).First();

        await Client.LoginAs(userWithPermission);
        var response = await Client.GetAsync($"/users/{orderOwner.Id}/orders/{customerOrder.Id}");
        var responseContent = await response.Content.ReadFromJsonAsync<OrderDetailedResponse>();

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        responseContent.EnsureCorrespondsTo(customerOrder);
    }
}
