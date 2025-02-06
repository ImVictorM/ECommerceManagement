using Application.Common.Security.Authentication;

using Domain.OrderAggregate;

using Contracts.Payments;
using Contracts.Orders;

using WebApi.Orders;
using WebApi.Payments;
using WebApi.Common.Utilities;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.Common.Seeds.Orders;
using IntegrationTests.Common.Seeds.Abstracts;
using IntegrationTests.TestUtils.Extensions.Http;

using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using System.Text;
using Xunit.Abstractions;
using FluentAssertions;

namespace IntegrationTests.Payments;

/// <summary>
/// Integration test for the process of handling a payment status changed notification.
/// </summary>
public class HandlePaymentStatusChangedTests : BaseIntegrationTest
{
    private readonly IHmacSignatureProvider _hmacSignatureProvider;
    private readonly IDataSeed<OrderSeedType, Order> _seedOrder;

    /// <summary>
    /// Initiates a new instance of the <see cref="HandlePaymentStatusChangedTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public HandlePaymentStatusChangedTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output) : base(factory, output)
    {
        _hmacSignatureProvider = factory.Services.GetRequiredService<IHmacSignatureProvider>();
        _seedOrder = SeedManager.GetSeed<OrderSeedType, Order>();
    }

    /// <summary>
    /// Verifies notifications without the X-Provider-Signature header returns bad request.
    /// </summary>
    [Fact]
    public async Task HandlePaymentStatusChanged_WithoutSignatureHeader_ReturnsBadRequest()
    {
        var request = new PaymentStatusChangedRequest("1", "Authorized");

        var response = await RequestService.Client.PostAsJsonAsync(PaymentWebhookEndpoints.BaseEndpoint, request);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    /// <summary>
    /// Verifies notifications with incorrect X-Provider-Signature header returns unauthorized.
    /// </summary>
    [Fact]
    public async Task HandlePaymentStatusChanged_WithIncorrectSignature_ReturnsUnauthorized()
    {
        var request = new PaymentStatusChangedRequest("1", "Authorized");

        var invalidBase64Signature = Convert.ToBase64String(Encoding.UTF8.GetBytes("xyz"));

        RequestService.Client.DefaultRequestHeaders.Add("X-Provider-Signature", invalidBase64Signature);

        var response = await RequestService.Client.PostAsJsonAsync(PaymentWebhookEndpoints.BaseEndpoint, request);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Verifies when the notification payment is not found is returned not found.
    /// </summary>
    [Fact]
    public async Task HandlePaymentStatusChanged_WhenPaymentIsNotFound_ReturnsNotFound()
    {
        var request = new PaymentStatusChangedRequest("404", "Authorized");

        var validSignature = _hmacSignatureProvider.ComputeHmac(JsonSerializerUtils.SerializeForWeb(request));

        RequestService.Client.DefaultRequestHeaders.Add("X-Provider-Signature", validSignature);

        var response = await RequestService.Client.PostAsJsonAsync(PaymentWebhookEndpoints.BaseEndpoint, request);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    /// <summary>
    /// Verifies when notifications have correct signature and payment exists a no content response is returned.
    /// </summary>
    [Fact]
    public async Task HandlePaymentStatusChanged_WithValidSignatureAndPaymentExists_ReturnsNoContent()
    {
        var existingPayment = await GetExistingOrderPayment();
        var request = new PaymentStatusChangedRequest(existingPayment.PaymentId, "Authorized");
        var validSignature = _hmacSignatureProvider.ComputeHmac(JsonSerializerUtils.SerializeForWeb(request));

        RequestService.Client.DefaultRequestHeaders.Add("X-Provider-Signature", validSignature);
        var response = await RequestService.Client.PostAsJsonAsync(PaymentWebhookEndpoints.BaseEndpoint, request);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
    }

    /// <summary>
    /// Verifies a bad request is returned when the notification payment status is not valid.
    /// </summary>
    [Fact]
    public async Task HandlePaymentStatusChanged_WithInvalidPaymentStatus_ReturnsBadRequest()
    {
        var invalidStatus = "invalid_status";
        var request = new PaymentStatusChangedRequest("1", invalidStatus);
        var validSignature = _hmacSignatureProvider.ComputeHmac(JsonSerializerUtils.SerializeForWeb(request));

        RequestService.Client.DefaultRequestHeaders.Add("X-Provider-Signature", validSignature);
        var response = await RequestService.Client.PostAsJsonAsync(PaymentWebhookEndpoints.BaseEndpoint, request);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    private async Task<OrderPaymentResponse> GetExistingOrderPayment()
    {
        var existingOrder = _seedOrder.GetByType(OrderSeedType.CUSTOMER_ORDER_PENDING);

        await RequestService.LoginAsAsync(UserSeedType.ADMIN);
        var order = await RequestService.Client.GetAsync($"{OrderEndpoints.BaseEndpoint}/{existingOrder.Id}");

        var orderContent = await order.Content.ReadRequiredFromJsonAsync<OrderDetailedResponse>();

        return orderContent.Payment!;
    }
}
