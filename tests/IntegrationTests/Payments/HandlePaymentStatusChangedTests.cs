using Application.Common.Security.Authentication;

using Domain.OrderAggregate;

using Contracts.Orders;

using WebApi.Common.Utilities;
using WebApi.Orders;
using WebApi.Payments;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.Common.Seeds.Orders;
using IntegrationTests.Common.Seeds.Abstracts;
using IntegrationTests.TestUtils.Extensions.Http;
using IntegrationTests.Payments.TestUtils;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Routing;
using System.Net.Http.Json;
using System.Text;
using Xunit.Abstractions;
using FluentAssertions;

namespace IntegrationTests.Payments;

/// <summary>
/// Integration tests for the handle payment status changed webhook feature.
/// </summary>
public class HandlePaymentStatusChangedTests : BaseIntegrationTest
{
    private readonly IHmacSignatureProvider _hmacSignatureProvider;
    private readonly IDataSeed<OrderSeedType, Order> _seedOrder;
    private readonly string? _endpoint;

    /// <summary>
    /// Initiates a new instance of the <see cref="HandlePaymentStatusChangedTests"/> class.
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
        _seedOrder = SeedManager.GetSeed<OrderSeedType, Order>();
        _endpoint = LinkGenerator.GetPathByName(
            nameof(PaymentWebhookEndpoints.HandlePaymentStatusChanged)
        );
    }

    /// <summary>
    /// Verifies notifications without the X-Provider-Signature header
    /// returns bad request.
    /// </summary>
    [Fact]
    public async Task HandlePaymentStatusChanged_WithoutSignatureHeader_ReturnsBadRequest()
    {
        var request = PaymentStatusChangedRequestUtils.CreateRequest();

        var response = await RequestService.Client.PostAsJsonAsync(
            _endpoint,
            request
        );

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
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

        RequestService.Client.DefaultRequestHeaders.Add(
            "X-Provider-Signature",
            invalidBase64Signature
        );
        var response = await RequestService.Client.PostAsJsonAsync(
            _endpoint,
            request
        );

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
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

        RequestService.Client.DefaultRequestHeaders.Add(
            "X-Provider-Signature",
            validSignature
        );

        var response = await RequestService.Client.PostAsJsonAsync(
            _endpoint,
            request
        );

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    /// <summary>
    /// Verifies when notifications have correct signature and existing payment id
    /// a no content response is returned.
    /// </summary>
    [Fact]
    public async Task HandlePaymentStatusChanged_WithValidSignatureAndExistingPayment_ReturnsNoContent()
    {
        var existingPayment = await GetExistingOrderPayment();

        var request = PaymentStatusChangedRequestUtils.CreateRequest(
            paymentId: existingPayment.PaymentId
        );

        var validSignature = _hmacSignatureProvider.ComputeHmac(
            JsonSerializerUtils.SerializeForWeb(request)
        );
 
        RequestService.Client.DefaultRequestHeaders.Add(
            "X-Provider-Signature",
            validSignature
        );

        var response = await RequestService.Client.PostAsJsonAsync(
            _endpoint,
            request
        );

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
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

        RequestService.Client.DefaultRequestHeaders.Add(
            "X-Provider-Signature",
            validSignature
        );

        var response = await RequestService.Client.PostAsJsonAsync(
            _endpoint,
            request
        );

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    private async Task<OrderPaymentResponse> GetExistingOrderPayment()
    {
        var existingOrder = _seedOrder.GetByType(OrderSeedType.CUSTOMER_ORDER_PENDING);

        var endpointGetOrderById = LinkGenerator.GetPathByName(
            nameof(OrderEndpoints.GetOrderById),
            new { id = existingOrder.Id.ToString() }
        );

        await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var order = await RequestService.Client.GetAsync(endpointGetOrderById);

        var orderContent = await order.Content
            .ReadRequiredFromJsonAsync<OrderDetailedResponse>();

        return orderContent.Payment;
    }
}
