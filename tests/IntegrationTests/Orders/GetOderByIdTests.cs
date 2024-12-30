using System.Net.Http.Json;
using Contracts.Orders;
using FluentAssertions;
using IntegrationTests.Common;
using IntegrationTests.TestUtils.Extensions.HttpClient;
using IntegrationTests.TestUtils.Seeds;
using WebApi.Endpoints;
using Xunit.Abstractions;

namespace IntegrationTests.Orders;

public class GetOderByIdTests : BaseIntegrationTest
{
    public GetOderByIdTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output) : base(factory, output)
    {
    }

    [Fact]
    public async Task GetOrderById_WhenOrderDoesNotExist_ReturnsNotFound()
    {
        var notFoundId = "404";

        await Client.LoginAs(SeedAvailableUsers.Admin);
        var response = await Client.GetAsync($"{OrderEndpoints.BaseEndpoint}/{notFoundId}");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetOrderById_WhenUserIsNotAuthenticated_ReturnsUnauthorized()
    {
        var existingOrder = OrderSeed.GetSeedOrder(SeedAvailableOrders.CUSTOMER_ORDER_PENDING);

        var response = await Client.GetAsync($"{OrderEndpoints.BaseEndpoint}/{existingOrder.Id}");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetOrderById_WhenUserIsNotAllowedToReadOrder_ReturnsForbidden()
    {
        var order = OrderSeed.GetSeedOrder(SeedAvailableOrders.CUSTOMER_ORDER_PENDING);

        await Client.LoginAs(SeedAvailableUsers.CustomerWithAddress);
        var response = await Client.GetAsync($"{OrderEndpoints.BaseEndpoint}/{order.Id}");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
    }

    [Theory]
    [InlineData(SeedAvailableUsers.Admin)]
    [InlineData(SeedAvailableUsers.Customer)]
    public async Task GetOrderById_WhenUserIsAllowedToReadOrder_ReturnsOk(SeedAvailableUsers allowedUser)
    {
        var order = OrderSeed.GetSeedOrder(SeedAvailableOrders.CUSTOMER_ORDER_PENDING);

        await Client.LoginAs(allowedUser);
        var response = await Client.GetAsync($"{OrderEndpoints.BaseEndpoint}/{order.Id}");
        var responseContent = await response.Content.ReadFromJsonAsync<OrderDetailedResponse>();

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        responseContent.Should().NotBeNull();
        responseContent!.Id.Should().Be(order.Id.ToString());
    }
}
