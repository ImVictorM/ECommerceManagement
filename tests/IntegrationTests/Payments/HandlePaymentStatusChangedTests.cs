using Contracts.Payments;
using Contracts.Orders;

using Infrastructure.Security.Authentication;
using Infrastructure.Security.Authentication.Settings;

using IntegrationTests.Common;
using IntegrationTests.TestUtils.Extensions.HttpClient;
using IntegrationTests.TestUtils.Seeds;

using WebApi.Orders;
using WebApi.Payments;
using WebApi.Common.Utilities;

using System.Net.Http.Json;
using System.Text;
using Xunit.Abstractions;
using Microsoft.Extensions.Options;
using FluentAssertions;

namespace IntegrationTests.Payments;

/// <summary>
/// Integration test for the process of handling a payment status changed notification.
/// </summary>
public class HandlePaymentStatusChangedTests : BaseIntegrationTest
{
    private readonly HmacSignatureProvider _hmacSignatureProvider;

    /// <summary>
    /// Initiates a new instance of the <see cref="HandlePaymentStatusChangedTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public HandlePaymentStatusChangedTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output) : base(factory, output)
    {
        // Same secret from appsettings.Test.json
        var testingHmacSignatureOptions = Options.Create(new HmacSignatureSettings
        {
            Secret = "test-secret-key"
        });

        _hmacSignatureProvider = new HmacSignatureProvider(testingHmacSignatureOptions);
    }

    /// <summary>
    /// Verifies notifications without the X-Provider-Signature header returns bad request.
    /// </summary>
    [Fact]
    public async Task HandlePaymentStatusChanged_WithoutSignatureHeader_ReturnsBadRequest()
    {
        var request = new PaymentStatusChangedRequest("1", "authorized");

        var response = await Client.PostAsJsonAsync(PaymentWebhookEndpoints.BaseEndpoint, request);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    /// <summary>
    /// Verifies notifications with incorrect X-Provider-Signature header returns unauthorized.
    /// </summary>
    [Fact]
    public async Task HandlePaymentStatusChanged_WithIncorrectSignature_ReturnsUnauthorized()
    {
        var request = new PaymentStatusChangedRequest("1", "authorized");

        var invalidBase64Signature = Convert.ToBase64String(Encoding.UTF8.GetBytes("xyz"));

        Client.DefaultRequestHeaders.Add("X-Provider-Signature", invalidBase64Signature);

        var response = await Client.PostAsJsonAsync(PaymentWebhookEndpoints.BaseEndpoint, request);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Verifies when the notification payment is not found is returned not found.
    /// </summary>
    [Fact]
    public async Task HandlePaymentStatusChanged_WhenPaymentIsNotFound_ReturnsNotFound()
    {
        var request = new PaymentStatusChangedRequest("404", "authorized");

        var validSignature = _hmacSignatureProvider.ComputeHmac(JsonSerializerUtils.SerializeForWeb(request));

        Client.DefaultRequestHeaders.Add("X-Provider-Signature", validSignature);

        var response = await Client.PostAsJsonAsync(PaymentWebhookEndpoints.BaseEndpoint, request);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    /// <summary>
    /// Verifies when notifications have correct signature and payment exists a no content response is returned.
    /// </summary>
    [Fact]
    public async Task HandlePaymentStatusChanged_WithValidSignatureAndPaymentExists_ReturnsNoContent()
    {
        var existingPayment = await GetExistingOrderPayment();
        var request = new PaymentStatusChangedRequest(existingPayment.PaymentId, "authorized");
        var validSignature = _hmacSignatureProvider.ComputeHmac(JsonSerializerUtils.SerializeForWeb(request));

        Client.DefaultRequestHeaders.Add("X-Provider-Signature", validSignature);
        var response = await Client.PostAsJsonAsync(PaymentWebhookEndpoints.BaseEndpoint, request);

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

        Client.DefaultRequestHeaders.Add("X-Provider-Signature", validSignature);
        var response = await Client.PostAsJsonAsync(PaymentWebhookEndpoints.BaseEndpoint, request);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    private async Task<OrderPaymentResponse> GetExistingOrderPayment()
    {
        var existingOrder = OrderSeed.GetSeedOrder(SeedAvailableOrders.CUSTOMER_ORDER_PENDING);

        await Client.LoginAs(SeedAvailableUsers.ADMIN);

        var order = await Client.GetAsync($"{OrderEndpoints.BaseEndpoint}/{existingOrder.Id}");
        var orderContent = await order.Content.ReadFromJsonAsync<OrderDetailedResponse>();

        return orderContent!.Payment!;
    }
}
