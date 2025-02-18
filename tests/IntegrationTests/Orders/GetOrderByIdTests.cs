using Domain.OrderAggregate;

using Contracts.Orders;

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
/// Integration tests for the process of getting an order by id.
/// </summary>
public class GetOrderByIdTests : BaseIntegrationTest
{
    private readonly IDataSeed<OrderSeedType, Order> _seedOrder;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetOrderByIdTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public GetOrderByIdTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output) : base(factory, output)
    {
        _seedOrder = SeedManager.GetSeed<OrderSeedType, Order>();
    }

    /// <summary>
    /// Tests when the order does not exists it is returned a not found response.
    /// </summary>
    [Fact]
    public async Task GetOrderById_WhenOrderDoesNotExist_ReturnsNotFound()
    {
        var notFoundId = "404";
        var endpoint = TestConstants.OrderEndpoints.GetOrderById(notFoundId);

        await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var response = await RequestService.Client.GetAsync(endpoint);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    /// <summary>
    /// Tests when the user is not authenticated an unauthorized response is returned.
    /// </summary>
    [Fact]
    public async Task GetOrderById_WhenUserIsNotAuthenticated_ReturnsUnauthorized()
    {
        var existingOrder = _seedOrder.GetByType(OrderSeedType.CUSTOMER_ORDER_PENDING);
        var endpoint = TestConstants.OrderEndpoints.GetOrderById(existingOrder.Id.ToString());

        var response = await RequestService.Client.GetAsync(endpoint);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Tests when the user is not authorized to read an order an forbidden response is returned.
    /// </summary>
    [Fact]
    public async Task GetOrderById_WhenUserIsNotAllowedToReadOrder_ReturnsForbidden()
    {
        var order = _seedOrder.GetByType(OrderSeedType.CUSTOMER_ORDER_PENDING);
        var endpoint = TestConstants.OrderEndpoints.GetOrderById(order.Id.ToString());

        await RequestService.LoginAsAsync(UserSeedType.CUSTOMER_WITH_ADDRESS);
        var response = await RequestService.Client.GetAsync(endpoint);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Tests when the user is allowed to read the order the an ok response is returned containing the order.
    /// </summary>
    /// <param name="allowedUser">Allowed user types.</param>
    [Theory]
    [InlineData(UserSeedType.ADMIN)]
    public async Task GetOrderById_WhenUserIsAllowed_ReturnsOk(UserSeedType allowedUser)
    {
        var order = _seedOrder.GetByType(OrderSeedType.CUSTOMER_ORDER_PENDING);
        var endpoint = TestConstants.OrderEndpoints.GetOrderById(order.Id.ToString());

        await RequestService.LoginAsAsync(allowedUser);
        var response = await RequestService.Client.GetAsync(endpoint);
        var responseContent = await response.Content.ReadRequiredFromJsonAsync<OrderDetailedResponse>();

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        responseContent.EnsureCorrespondsTo(order);
    }
}
