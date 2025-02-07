namespace IntegrationTests.TestUtils.Constants;

public static partial class TestConstants
{
    /// <summary>
    /// Defines the endpoints for the payment resources.
    /// </summary>
    public static class PaymentEndpoints
    {
        /// <summary>
        /// Webhook endpoint to respond to payment status change.
        /// </summary>
        public const string PaymentStatusChanged = "webhooks/payments";
    }
}
