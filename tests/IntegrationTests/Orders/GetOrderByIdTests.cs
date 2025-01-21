using Contracts.Orders;

using IntegrationTests.Common;
using IntegrationTests.TestUtils.Extensions.HttpClient;
using IntegrationTests.TestUtils.Seeds;

using WebApi.Endpoints;

using System.Net.Http.Json;
using FluentAssertions;
using Xunit.Abstractions;

namespace IntegrationTests.Orders;

/// <summary>
/// Integration tests for the process of getting an order by id.
/// </summary>
public class GetOrderByIdTests : BaseIntegrationTest
{
    /// <summary>
    /// Initiates a new instance of the <see cref="PlaceOrderTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public GetOrderByIdTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output) : base(factory, output)
    {
    }

    /// <summary>
    /// Tests when the order does not exists it is returned a not found response.
    /// </summary>
    [Fact]
    public async Task GetOrderById_WhenOrderDoesNotExist_ReturnsNotFound()
    {
        var notFoundId = "404";

        await Client.LoginAs(SeedAvailableUsers.Admin);
        var response = await Client.GetAsync($"{OrderEndpoints.BaseEndpoint}/{notFoundId}");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    /// <summary>
    /// Tests when the user is not authenticated an unauthorized response is returned.
    /// </summary>
    [Fact]
    public async Task GetOrderById_WhenUserIsNotAuthenticated_ReturnsUnauthorized()
    {
        var existingOrder = OrderSeed.GetSeedOrder(SeedAvailableOrders.CUSTOMER_ORDER_PENDING);

        var response = await Client.GetAsync($"{OrderEndpoints.BaseEndpoint}/{existingOrder.Id}");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Tests when the user is not authorized to read an order an forbidden response is returned.
    /// </summary>
    [Fact]
    public async Task GetOrderById_WhenUserIsNotAllowedToReadOrder_ReturnsForbidden()
    {
        var order = OrderSeed.GetSeedOrder(SeedAvailableOrders.CUSTOMER_ORDER_PENDING);

        await Client.LoginAs(SeedAvailableUsers.CustomerWithAddress);
        var response = await Client.GetAsync($"{OrderEndpoints.BaseEndpoint}/{order.Id}");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Tests when the user is allowed to read the order the an ok response is returned containing the order.
    /// </summary>
    /// <param name="allowedUser">Allowed user types.</param>
    [Theory]
    [InlineData(SeedAvailableUsers.Admin)]
    public async Task GetOrderById_WhenUserIsAllowed_ReturnsOk(SeedAvailableUsers allowedUser)
    {
        var order = OrderSeed.GetSeedOrder(SeedAvailableOrders.CUSTOMER_ORDER_PENDING);

        await Client.LoginAs(allowedUser);
        var response = await Client.GetAsync($"{OrderEndpoints.BaseEndpoint}/{order.Id}");
        var responseContent = await response.Content.ReadFromJsonAsync<OrderDetailedResponse>();

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        responseContent.Should().NotBeNull();
        responseContent!.Id.Should().Be(order.Id.ToString());
        responseContent!.Payment.Should().NotBeNull();
    }
}
