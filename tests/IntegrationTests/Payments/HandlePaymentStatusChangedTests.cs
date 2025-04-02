using Application.Common.Security.Authentication;

using Contracts.Orders;
using WebApi.Orders;
using WebApi.Payments;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.Common.Seeds.Orders;
using IntegrationTests.TestUtils.Extensions.Http;
using IntegrationTests.Payments.TestUtils;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Routing;
using System.Net.Http.Json;
using System.Text;
using Xunit.Abstractions;
using FluentAssertions;
using System.Net;
using WebApi.Common.Utils;

namespace IntegrationTests.Payments;

/// <summary>
/// Integration tests for the handle payment status changed webhook feature.
/// </summary>
public class HandlePaymentStatusChangedTests : BaseIntegrationTest
{
    private readonly IHmacSignatureProvider _hmacSignatureProvider;
    private readonly IOrderSeed _seedOrder;
    private readonly string? _endpoint;
    private readonly HttpClient _client;

    /// <summary>
    /// Initiates a new instance of the
    /// <see cref="HandlePaymentStatusChangedTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public HandlePaymentStatusChangedTests(
        IntegrationTestWebAppFactory factory,
        ITestOutputHelper output
    ) : base(factory, output)
    {
        _hmacSignatureProvider = factory.Services
            .GetRequiredService<IHmacSignatureProvider>();

        _seedOrder = SeedManager.GetSeed<IOrderSeed>();

        _endpoint = LinkGenerator.GetPathByName(
            nameof(PaymentWebhookEndpoints.HandlePaymentStatusChanged)
        );

        _client = RequestService.CreateClient();
    }

    /// <summary>
    /// Verifies notifications without the X-Provider-Signature header
    /// returns bad request.
    /// </summary>
    [Fact]
    public async Task HandlePaymentStatusChanged_WithoutSignatureHeader_ReturnsBadRequest()
    {
        var request = PaymentStatusChangedRequestUtils.CreateRequest();

        var response = await _client.PostAsJsonAsync(
            _endpoint,
            request
        );

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    /// <summary>
    /// Verifies notifications with incorrect X-Provider-Signature
    /// header returns unauthorized.
    /// </summary>
    [Fact]
    public async Task HandlePaymentStatusChanged_WithIncorrectSignature_ReturnsUnauthorized()
    {
        var request = PaymentStatusChangedRequestUtils.CreateRequest();

        var invalidBase64Signature = Convert.ToBase64String(
            Encoding.UTF8.GetBytes("xyz")
        );

        _client.SetProviderSignatureHeader(invalidBase64Signature);

        var response = await _client.PostAsJsonAsync(
            _endpoint,
            request
        );

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Verifies when the notification payment is not found a not found response
    /// is returned.
    /// </summary>
    [Fact]
    public async Task HandlePaymentStatusChanged_WhenPaymentIsNotFound_ReturnsNotFound()
    {
        var request = PaymentStatusChangedRequestUtils.CreateRequest(
            paymentId: "404"
        );

        var validSignature = _hmacSignatureProvider.ComputeHmac(
            JsonSerializerUtils.SerializeForWeb(request)
        );

        _client.SetProviderSignatureHeader(validSignature);

        var response = await _client.PostAsJsonAsync(
            _endpoint,
            request
        );

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    /// <summary>
    /// Verifies when notifications have correct signature and existent payment
    /// a no content response is returned.
    /// </summary>
    [Fact]
    public async Task HandlePaymentStatusChanged_WithValidSignatureAndExistentPayment_ReturnsNoContent()
    {
        var existentPayment = await GetExistentOrderPayment();

        var request = PaymentStatusChangedRequestUtils.CreateRequest(
            paymentId: existentPayment.PaymentId
        );

        var validSignature = _hmacSignatureProvider.ComputeHmac(
            JsonSerializerUtils.SerializeForWeb(request)
        );

        _client.SetProviderSignatureHeader(validSignature);

        var response = await _client.PostAsJsonAsync(
            _endpoint,
            request
        );

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    /// <summary>
    /// Verifies a bad request is returned when the notification
    /// payment status is not valid.
    /// </summary>
    [Fact]
    public async Task HandlePaymentStatusChanged_WithInvalidPaymentStatus_ReturnsBadRequest()
    {
        var invalidStatus = "invalid_status";
        var request = PaymentStatusChangedRequestUtils.CreateRequest(
            paymentStatus: invalidStatus
        );

        var validSignature = _hmacSignatureProvider.ComputeHmac(
            JsonSerializerUtils.SerializeForWeb(request)
        );

        _client.SetProviderSignatureHeader(validSignature);

        var response = await _client.PostAsJsonAsync(
            _endpoint,
            request
        );

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    private async Task<OrderPaymentResponse> GetExistentOrderPayment()
    {
        var existentOrder = _seedOrder.GetEntity(OrderSeedType.CUSTOMER_ORDER_PENDING);

        var endpointGetOrderById = LinkGenerator.GetPathByName(
            nameof(OrderEndpoints.GetOrderById),
            new { id = existentOrder.Id.ToString() }
        );

        var client = await RequestService.LoginAsAsync(UserSeedType.ADMIN);

        var order = await client.GetAsync(endpointGetOrderById);

        var orderContent = await order.Content
            .ReadRequiredFromJsonAsync<OrderDetailedResponse>();

        return orderContent.Payment;
    }
}
