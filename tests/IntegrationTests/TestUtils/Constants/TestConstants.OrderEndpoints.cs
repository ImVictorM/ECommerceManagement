namespace IntegrationTests.TestUtils.Constants;

public static partial class TestConstants
{
    /// <summary>
    /// Defines the endpoints for the order resources.
    /// </summary>
    public static class OrderEndpoints
    {
        private const string BaseEndpoint = "/orders";
        private static string ResourceWithIdentifier(string id) => $"{BaseEndpoint}/{id}";

        /// <summary>
        /// Endpoint to place an order.
        /// </summary>
        public const string PlaceOrder = BaseEndpoint;
        /// <summary>
        /// Endpoint to get all orders.
        /// </summary>
        public const string GetOrders = BaseEndpoint;
        /// <summary>
        /// Endpoint to get an order by id.
        /// </summary>
        public static string GetOrderById(string id) => ResourceWithIdentifier(id);
    }
}
