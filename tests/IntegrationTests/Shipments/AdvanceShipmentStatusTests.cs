using Contracts.Orders;

using Application.Common.Security.Authentication;

using Domain.OrderAggregate.ValueObjects;
using Domain.ShipmentAggregate.Enumerations;
using Domain.ShipmentAggregate.ValueObjects;
using Domain.PaymentAggregate.Enumerations;

using WebApi.Common.Utilities;
using WebApi.Orders;
using WebApi.Payments;
using WebApi.Shipments;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Orders;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.Common.Seeds.Carriers;
using IntegrationTests.Payments.TestUtils;
using IntegrationTests.TestUtils.Extensions.Http;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Routing;
using FluentAssertions;
using Xunit.Abstractions;
using System.Net.Http.Json;
using System.Net;

namespace IntegrationTests.Shipments;

/// <summary>
/// Integration tests for the advance shipment status feature.
/// </summary>
public class AdvanceShipmentStatusTests : BaseIntegrationTest
{
    private readonly IOrderSeed _seedOrder;
    private readonly IHmacSignatureProvider _hmacSignatureProvider;

    /// <summary>
    /// Initiates a new instance of the <see cref="AdvanceShipmentStatusTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public AdvanceShipmentStatusTests(
        IntegrationTestWebAppFactory factory,
        ITestOutputHelper output
    ) : base(factory, output)
    {
        _seedOrder = SeedManager.GetSeed<IOrderSeed>();
        _hmacSignatureProvider = factory.Services
            .GetRequiredService<IHmacSignatureProvider>();
    }

    /// <summary>
    /// Verifies it is not possible to advance a shipment status without authorization.
    /// </summary>
    [Fact]
    public async Task AdvanceShipmentStatus_WithoutAuthorization_ReturnsUnauthorized()
    {
        var endpoint = LinkGenerator.GetPathByName(
            nameof(ShipmentEndpoints.AdvanceShipmentStatus),
            new { id = "1" }
        );

        var response = await RequestService
            .CreateClient()
            .PatchAsync(endpoint, null);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Verifies it is not possible to advance a shipment status without the carrier role.
    /// </summary>
    [Fact]
    public async Task AdvanceShipmentStatus_WithoutCarrierRole_ReturnsForbidden()
    {
        var endpoint = LinkGenerator.GetPathByName(
            nameof(ShipmentEndpoints.AdvanceShipmentStatus),
            new { id = "1" }
        );

        var client = await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var response = await client.PatchAsync(endpoint, null);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Verifies a not found error is returned when the shipment does not exist.
    /// </summary>
    [Fact]
    public async Task AdvanceShipmentStatus_WhenShipmentDoesNotExist_ReturnsNotFound()
    {
        var invalidShipmentId = "999999";

        var endpoint = LinkGenerator.GetPathByName(
            nameof(ShipmentEndpoints.AdvanceShipmentStatus),
            new { id = invalidShipmentId }
        );

        var client = await RequestService.LoginAsAsync(CarrierSeedType.INTERNAL);
        var response = await client.PatchAsync(
            endpoint,
            null
        );

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    /// <summary>
    /// Verifies the shipment status is advanced when the authenticated
    /// user has the permissions required, the shipment exists,
    /// and the shipment status is not pending.
    /// </summary>
    [Fact]
    public async Task AdvanceShipmentStatus_WithoutPendingShipment_ReturnsNoContent()
    {
        var orderPending = _seedOrder.GetEntity(OrderSeedType.CUSTOMER_ORDER_PENDING);
        var orderPendingDetails = await GetOrderDetailsById(orderPending.Id);
        var shipmentId = ShipmentId.Create(orderPendingDetails.Shipment.ShipmentId);

        var endpoint = LinkGenerator.GetPathByName(
            nameof(ShipmentEndpoints.AdvanceShipmentStatus),
            new { id = shipmentId.ToString() }
        );

        await PayOrder(orderPendingDetails);

        var client = await RequestService.LoginAsAsync(CarrierSeedType.INTERNAL);

        var response = await client.PatchAsync(
            endpoint,
            null
        );
        var responseOrderDetailed = await GetOrderDetailsById(orderPending.Id);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        responseOrderDetailed.Shipment.Status
            .Should()
            .Be(ShipmentStatus.Shipped.Name);
    }

    /// <summary>
    /// Verifies it is not possible to advance manually a pending order.
    /// </summary>
    [Fact]
    public async Task AdvanceShipmentStatus_WithPendingShipment_ReturnsBadRequest()
    {
        var orderPending = _seedOrder.GetEntity(OrderSeedType.CUSTOMER_ORDER_PENDING);
        var orderPendingDetails = await GetOrderDetailsById(orderPending.Id);
        var shipmentId = ShipmentId.Create(orderPendingDetails.Shipment.ShipmentId);

        var endpoint = LinkGenerator.GetPathByName(
            nameof(ShipmentEndpoints.AdvanceShipmentStatus),
            new { id = shipmentId.ToString() }
        );

        var client = await RequestService.LoginAsAsync(CarrierSeedType.INTERNAL);
        var response = await client.PatchAsync(endpoint, null);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    private async Task<OrderDetailedResponse> GetOrderDetailsById(OrderId id)
    {
        var endpointGetOrderById = LinkGenerator.GetPathByName(
            nameof(OrderEndpoints.GetOrderById),
            new { id = id.ToString() }
        );

        var client = await RequestService.LoginAsAsync(UserSeedType.ADMIN);

        var response = await client.GetAsync(endpointGetOrderById);

        var orderDetails = await response.Content
            .ReadRequiredFromJsonAsync<OrderDetailedResponse>();

        return orderDetails;
    }

    private async Task PayOrder(OrderDetailedResponse order)
    {
        var existingPayment = order.Payment;

        var endpoint = LinkGenerator.GetPathByName(
            nameof(PaymentWebhookEndpoints.HandlePaymentStatusChanged)
        );

        var request = PaymentStatusChangedRequestUtils.CreateRequest(
           paymentId: existingPayment.PaymentId,
           paymentStatus: PaymentStatus.Authorized.Name
        );

        var validSignature = _hmacSignatureProvider
            .ComputeHmac(JsonSerializerUtils.SerializeForWeb(request));

        var client = RequestService.CreateClient();

        client.SetProviderSignatureHeader(validSignature);

        var response = await client.PostAsJsonAsync(
            endpoint,
            request
        );

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
