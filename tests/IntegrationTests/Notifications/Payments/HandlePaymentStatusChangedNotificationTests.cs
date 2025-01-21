using Contracts.Notifications;
using Contracts.Orders;

using WebApi.Common.Utils;
using WebApi.Endpoints;
using WebApi.Endpoints.Notifications;

using Infrastructure.Security.Authentication;
using Infrastructure.Security.Authentication.Settings;

using IntegrationTests.Common;
using IntegrationTests.TestUtils.Extensions.HttpClient;
using IntegrationTests.TestUtils.Seeds;

using System.Net.Http.Json;
using System.Text;
using Xunit.Abstractions;
using Microsoft.Extensions.Options;
using FluentAssertions;

namespace IntegrationTests.Notifications.Payments;

/// <summary>
/// Integration test for the process of handling a payment status changed notification.
/// </summary>
public class HandlePaymentStatusChangedNotificationTests : BaseIntegrationTest
{
    private readonly HmacSignatureProvider _hmacSignatureProvider;

    /// <summary>
    /// Initiates a new instance of the <see cref="HandlePaymentStatusChangedNotificationTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public HandlePaymentStatusChangedNotificationTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output) : base(factory, output)
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
        var request = new PaymentStatusChangedNotification("1", "authorized");

        var response = await Client.PostAsJsonAsync(PaymentNotificationEndpoints.BaseEndpoint, request);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    /// <summary>
    /// Verifies notifications with incorrect X-Provider-Signature header returns unauthorized.
    /// </summary>
    [Fact]
    public async Task HandlePaymentStatusChanged_WithIncorrectSignature_ReturnsUnauthorized()
    {
        var request = new PaymentStatusChangedNotification("1", "authorized");

        var invalidBase64Signature = Convert.ToBase64String(Encoding.UTF8.GetBytes("xyz"));

        Client.DefaultRequestHeaders.Add("X-Provider-Signature", invalidBase64Signature);

        var response = await Client.PostAsJsonAsync(PaymentNotificationEndpoints.BaseEndpoint, request);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Verifies when the notification payment is not found is returned not found.
    /// </summary>
    [Fact]
    public async Task HandlePaymentStatusChanged_WhenPaymentIsNotFound_ReturnsNotFound()
    {
        var request = new PaymentStatusChangedNotification("404", "authorized");

        var validSignature = _hmacSignatureProvider.ComputeHmac(JsonSerializerUtils.SerializeForWeb(request));

        Client.DefaultRequestHeaders.Add("X-Provider-Signature", validSignature);

        var response = await Client.PostAsJsonAsync(PaymentNotificationEndpoints.BaseEndpoint, request);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    /// <summary>
    /// Verifies when notifications have correct signature and payment exists a no content response is returned.
    /// </summary>
    [Fact]
    public async Task HandlePaymentStatusChanged_WithValidSignatureAndPaymentExists_ReturnsNoContent()
    {
        var existingPayment = await GetExistingOrderPayment();
        var request = new PaymentStatusChangedNotification(existingPayment.PaymentId, "authorized");
        var validSignature = _hmacSignatureProvider.ComputeHmac(JsonSerializerUtils.SerializeForWeb(request));

        Client.DefaultRequestHeaders.Add("X-Provider-Signature", validSignature);
        var response = await Client.PostAsJsonAsync(PaymentNotificationEndpoints.BaseEndpoint, request);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
    }

    /// <summary>
    /// Verifies a bad request is returned when the notification payment status is not valid.
    /// </summary>
    [Fact]
    public async Task HandlePaymentStatusChanged_WithInvalidPaymentStatus_ReturnsBadRequest()
    {
        var invalidStatus = "invalid_status";
        var request = new PaymentStatusChangedNotification("1", invalidStatus);
        var validSignature = _hmacSignatureProvider.ComputeHmac(JsonSerializerUtils.SerializeForWeb(request));

        Client.DefaultRequestHeaders.Add("X-Provider-Signature", validSignature);
        var response = await Client.PostAsJsonAsync(PaymentNotificationEndpoints.BaseEndpoint, request);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    private async Task<OrderPaymentResponse> GetExistingOrderPayment()
    {
        var existingOrder = OrderSeed.GetSeedOrder(SeedAvailableOrders.CUSTOMER_ORDER_PENDING);

        await Client.LoginAs(SeedAvailableUsers.Admin);

        var order = await Client.GetAsync($"{OrderEndpoints.BaseEndpoint}/{existingOrder.Id}");
        var orderContent = await order.Content.ReadFromJsonAsync<OrderDetailedResponse>();

        return orderContent!.Payment!;
    }
}
