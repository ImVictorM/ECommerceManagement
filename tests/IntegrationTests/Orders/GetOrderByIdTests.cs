using Domain.OrderAggregate;

using Contracts.Orders;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.Common.Seeds.Abstracts;
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
    private readonly IDataSeed<OrderSeedType, Order> _seedOrder;

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
        _seedOrder = SeedManager.GetSeed<OrderSeedType, Order>();
    }

    /// <summary>
    /// Verifies when the order does not exists it is
    /// returned a not found response.
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
    /// Verifies when the user is not authenticated an
    /// unauthorized response is returned.
    /// </summary>
    [Fact]
    public async Task GetOrderById_WhenUserIsNotAuthenticated_ReturnsUnauthorized()
    {
        var existingOrder = _seedOrder.GetByType(
            OrderSeedType.CUSTOMER_ORDER_PENDING
        );

        var endpoint = LinkGenerator.GetPathByName(
            nameof(OrderEndpoints.GetOrderById),
            new
            {
                id = existingOrder.Id.ToString(),
            }
        );

        var response = await RequestService.CreateClient().GetAsync(endpoint);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Verifies when the user is not authorized to read an order
    /// an forbidden response is returned.
    /// </summary>
    [Fact]
    public async Task GetOrderById_WhenUserIsNotAllowedToReadOrder_ReturnsForbidden()
    {
        var order = _seedOrder.GetByType(OrderSeedType.CUSTOMER_ORDER_PENDING);

        var endpoint = LinkGenerator.GetPathByName(
            nameof(OrderEndpoints.GetOrderById),
            new
            {
                id = order.Id.ToString(),
            }
        );

        var client = await RequestService.LoginAsAsync(
            UserSeedType.CUSTOMER_WITH_ADDRESS
        );
        var response = await client.GetAsync(endpoint);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Verifies when the user is allowed to read the order the an
    /// ok response is returned containing the order.
    /// </summary>
    /// <param name="allowedUser">Allowed user types.</param>
    [Theory]
    [InlineData(UserSeedType.ADMIN)]
    public async Task GetOrderById_WhenUserIsAllowed_ReturnsOk(
        UserSeedType allowedUser
    )
    {
        var order = _seedOrder.GetByType(OrderSeedType.CUSTOMER_ORDER_PENDING);

        var endpoint = LinkGenerator.GetPathByName(
            nameof(OrderEndpoints.GetOrderById),
            new
            {
                id = order.Id.ToString(),
            }
        );

        var client = await RequestService.LoginAsAsync(allowedUser);
        var response = await client.GetAsync(endpoint);
        var responseContent = await response.Content
            .ReadRequiredFromJsonAsync<OrderDetailedResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent.EnsureCorrespondsTo(order);
    }
}
