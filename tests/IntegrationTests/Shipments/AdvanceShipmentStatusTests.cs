using Contracts.Orders;

using Domain.OrderAggregate;
using Domain.OrderAggregate.ValueObjects;
using Domain.ShipmentAggregate.Enumerations;
using Domain.ShipmentAggregate.ValueObjects;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Abstracts;
using IntegrationTests.Common.Seeds.Orders;
using IntegrationTests.TestUtils.Constants;
using IntegrationTests.TestUtils.Extensions.Http;

using FluentAssertions;
using Xunit.Abstractions;

namespace IntegrationTests.Shipments;

/// <summary>
/// Integration tests for the advance shipment status feature.
/// </summary>
public class AdvanceShipmentStatusTests : BaseIntegrationTest
{
    private readonly IDataSeed<OrderSeedType, Order> _seedOrder;

    /// <summary>
    /// Initiates a new instance of the <see cref="AdvanceShipmentStatusTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public AdvanceShipmentStatusTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output) : base(factory, output)
    {
        _seedOrder = SeedManager.GetSeed<OrderSeedType, Order>();
    }

    /// <summary>
    /// Verifies it is not possible to advance a shipment status without authorization.
    /// </summary>
    [Fact]
    public async Task AdvanceShipmentStatus_WithoutAuthorization_ReturnsUnauthorized()
    {
        var endpoint = TestConstants.ShipmentEndpoints.AdvanceShipmentStatus("1");

        var response = await RequestService.Client.PatchAsync(endpoint, null);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Verifies it is not possible to advance a shipment status without the carrier role.
    /// </summary>
    [Fact]
    public async Task AdvanceShipmentStatus_WithoutCarrierRole_ReturnsForbidden()
    {
        var endpoint = TestConstants.ShipmentEndpoints.AdvanceShipmentStatus("1");

        await RequestService.LoginAsAsync(Common.Seeds.Users.UserSeedType.ADMIN);
        var response = await RequestService.Client.PatchAsync(endpoint, null);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Verifies a not found error is returned when the shipment does not exist.
    /// </summary>
    [Fact]
    public async Task AdvanceShipmentStatus_WhenShipmentDoesNotExist_ReturnsNotFound()
    {
        var invalidShipmentId = "999999";

        var endpoint = TestConstants.ShipmentEndpoints.AdvanceShipmentStatus(invalidShipmentId);

        await RequestService.LoginAsAsync(Common.Seeds.Carriers.CarrierSeedType.INTERNAL);
        var response = await RequestService.Client.PatchAsync(endpoint, null);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    /// <summary>
    /// Verifies the shipment status is advanced when the authenticated
    /// user has the permissions required and the shipment exists.
    /// </summary>
    [Fact]
    public async Task AdvanceShipmentStatus_WithAuthorizationAndExistingShipment_AdvancesShipmentStatusAndReturnsNoContent()
    {
        var orderPending = _seedOrder.GetByType(OrderSeedType.CUSTOMER_ORDER_PENDING);
        var shipmentId = await GetShipmentIdFromOrderDetails(orderPending);

        var endpoint = TestConstants.ShipmentEndpoints.AdvanceShipmentStatus(shipmentId.ToString());

        await RequestService.LoginAsAsync(Common.Seeds.Carriers.CarrierSeedType.INTERNAL);
        var response = await RequestService.Client.PatchAsync(endpoint, null);
        var orderDetailedResponse = await GetOrderDetailsById(orderPending.Id);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        orderDetailedResponse.Shipment.Status.Should().Be(ShipmentStatus.Preparing.Name);
    }

    private async Task<OrderDetailedResponse> GetOrderDetailsById(OrderId id)
    {
        await RequestService.LoginAsAsync(Common.Seeds.Users.UserSeedType.ADMIN);
        var orderByIdEndpoint = TestConstants.OrderEndpoints.GetOrderById(id.ToString());
        var response = await RequestService.Client.GetAsync(orderByIdEndpoint);
        var orderDetails = await response.Content.ReadRequiredFromJsonAsync<OrderDetailedResponse>();

        return orderDetails;
    }

    private async Task<ShipmentId> GetShipmentIdFromOrderDetails(Order order)
    {
        var orderDetails = await GetOrderDetailsById(order.Id);

        return ShipmentId.Create(orderDetails.Shipment.ShipmentId);
    }
}
