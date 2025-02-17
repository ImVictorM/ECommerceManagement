using Contracts.Orders;

using Application.Common.Security.Authentication;

using Domain.OrderAggregate;
using Domain.OrderAggregate.ValueObjects;
using Domain.ShipmentAggregate.Enumerations;
using Domain.ShipmentAggregate.ValueObjects;

using WebApi.Common.Utilities;

using Contracts.Payments;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Abstracts;
using IntegrationTests.Common.Seeds.Orders;
using IntegrationTests.TestUtils.Constants;
using IntegrationTests.TestUtils.Extensions.Http;

using FluentAssertions;
using Xunit.Abstractions;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests.Shipments;

/// <summary>
/// Integration tests for the advance shipment status feature.
/// </summary>
public class AdvanceShipmentStatusTests : BaseIntegrationTest
{
    private readonly IDataSeed<OrderSeedType, Order> _seedOrder;
    private readonly IHmacSignatureProvider _hmacSignatureProvider;

    /// <summary>
    /// Initiates a new instance of the <see cref="AdvanceShipmentStatusTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public AdvanceShipmentStatusTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output) : base(factory, output)
    {
        _seedOrder = SeedManager.GetSeed<OrderSeedType, Order>();
        _hmacSignatureProvider = factory.Services.GetRequiredService<IHmacSignatureProvider>();
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
    /// user has the permissions required, the shipment exists, and the shipment status is not pending.
    /// </summary>
    [Fact]
    public async Task AdvanceShipmentStatus_WithAuthorizationAndExistingShipment_AdvancesShipmentStatusAndReturnsNoContent()
    {
        var orderPending = _seedOrder.GetByType(OrderSeedType.CUSTOMER_ORDER_PENDING);
        var orderPendingDetails = await GetOrderDetailsById(orderPending.Id);
        var shipmentId = ShipmentId.Create(orderPendingDetails.Shipment.ShipmentId);

        var endpoint = TestConstants.ShipmentEndpoints.AdvanceShipmentStatus(shipmentId.ToString());

        await PayOrder(orderPendingDetails);
        await RequestService.LoginAsAsync(Common.Seeds.Carriers.CarrierSeedType.INTERNAL);
        var response = await RequestService.Client.PatchAsync(endpoint, null);
        var orderDetailedResponse = await GetOrderDetailsById(orderPending.Id);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        orderDetailedResponse.Shipment.Status.Should().Be(ShipmentStatus.Shipped.Name);
    }

    /// <summary>
    /// Verifies it is not possible to advance manually a pending order.
    /// </summary>
    [Fact]
    public async Task AdvanceShipmentStatus_WithAuthorizationAndExistingPendingShipment_ReturnsBadRequest()
    {
        var orderPending = _seedOrder.GetByType(OrderSeedType.CUSTOMER_ORDER_PENDING);
        var orderPendingDetails = await GetOrderDetailsById(orderPending.Id);
        var shipmentId = ShipmentId.Create(orderPendingDetails.Shipment.ShipmentId);

        var endpoint = TestConstants.ShipmentEndpoints.AdvanceShipmentStatus(shipmentId.ToString());

        await RequestService.LoginAsAsync(Common.Seeds.Carriers.CarrierSeedType.INTERNAL);
        var response = await RequestService.Client.PatchAsync(endpoint, null);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    private async Task<OrderDetailedResponse> GetOrderDetailsById(OrderId id)
    {
        await RequestService.LoginAsAsync(Common.Seeds.Users.UserSeedType.ADMIN);
        var orderByIdEndpoint = TestConstants.OrderEndpoints.GetOrderById(id.ToString());
        var response = await RequestService.Client.GetAsync(orderByIdEndpoint);
        var orderDetails = await response.Content.ReadRequiredFromJsonAsync<OrderDetailedResponse>();

        return orderDetails;
    }

    private async Task PayOrder(OrderDetailedResponse order)
    {
        var existingPayment = order.Payment;
        var request = new PaymentStatusChangedRequest(existingPayment.PaymentId, "Authorized");
        var validSignature = _hmacSignatureProvider.ComputeHmac(JsonSerializerUtils.SerializeForWeb(request));
        var endpoint = TestConstants.PaymentEndpoints.PaymentStatusChanged;

        RequestService.Client.DefaultRequestHeaders.Add("X-Provider-Signature", validSignature);
        var response = await RequestService.Client.PostAsJsonAsync(endpoint, request);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
    }
}
