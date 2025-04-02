using Contracts.Orders;

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
/// Integration tests for the get order by id feature.
/// </summary>
public class GetOrderByIdTests : BaseIntegrationTest
{
    private readonly IOrderSeed _seedOrder;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetOrderByIdTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public GetOrderByIdTests(
        IntegrationTestWebAppFactory factory,
        ITestOutputHelper output
    ) : base(factory, output)
    {
        _seedOrder = SeedManager.GetSeed<IOrderSeed>();
    }

    /// <summary>
    /// Verifies a not found response is returned when the order does not exist.
    /// </summary>
    [Fact]
    public async Task GetOrderById_WhenOrderDoesNotExist_ReturnsNotFound()
    {
        var notFoundId = "404";

        var endpoint = LinkGenerator.GetPathByName(
            nameof(OrderEndpoints.GetOrderById),
            new
            {
                id = notFoundId,
            }
        );

        var client = await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var response = await client.GetAsync(endpoint);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    /// <summary>
    /// Verifies an unauthorized response is returned when the user is not
    /// authenticated.
    /// </summary>
    [Fact]
    public async Task GetOrderById_WithoutAuthentication_ReturnsUnauthorized()
    {
        var idExistentOrder = _seedOrder
            .GetEntityId(OrderSeedType.CUSTOMER_ORDER_PENDING)
            .ToString();

        var endpoint = LinkGenerator.GetPathByName(
            nameof(OrderEndpoints.GetOrderById),
            new
            {
                id = idExistentOrder,
            }
        );

        var response = await RequestService.CreateClient().GetAsync(endpoint);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Verifies a forbidden response is returned when the user is not an
    /// administrator.
    /// </summary>
    [Fact]
    public async Task GetOrderById_WithoutAdminAuthentication_ReturnsForbidden()
    {
        var existentOrderId = _seedOrder
            .GetEntityId(OrderSeedType.CUSTOMER_ORDER_PENDING)
            .ToString();

        var endpoint = LinkGenerator.GetPathByName(
            nameof(OrderEndpoints.GetOrderById),
            new
            {
                id = existentOrderId,
            }
        );

        var client = await RequestService.LoginAsAsync(
            UserSeedType.CUSTOMER_WITH_ADDRESS
        );
        var response = await client.GetAsync(endpoint);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Verifies an OK response is returned when the user is an administrator.
    /// </summary>
    /// <param name="allowedUserType">The allowed user type.</param>
    [Theory]
    [InlineData(UserSeedType.ADMIN)]
    public async Task GetOrderById_WhenUserIsAllowed_ReturnsOk(
        UserSeedType allowedUserType
    )
    {
        var order = _seedOrder.GetEntity(OrderSeedType.CUSTOMER_ORDER_PENDING);

        var endpoint = LinkGenerator.GetPathByName(
            nameof(OrderEndpoints.GetOrderById),
            new
            {
                id = order.Id.ToString(),
            }
        );

        var client = await RequestService.LoginAsAsync(allowedUserType);
        var response = await client.GetAsync(endpoint);
        var responseContent = await response.Content
            .ReadRequiredFromJsonAsync<OrderDetailedResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent.EnsureCorrespondsTo(order);
    }
}
