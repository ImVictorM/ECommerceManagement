namespace IntegrationTests.TestUtils.Constants;

public static partial class TestConstants
{
    /// <summary>
    /// Defines the endpoints for the customer order resources.
    /// </summary>
    public static class CustomerOrderEndpoints
    {
        /// <summary>
        /// Endpoint to get all the customer orders.
        /// </summary>
        public static string GetCustomerOrders(string customerId)
            => $"users/customers/{customerId}/orders";
        /// <summary>
        /// Endpoint to get a customer's order by id.
        /// </summary>
        public static string GetCustomerOrderById(string customerId, string orderId)
            => $"users/customers/{customerId}/orders/{orderId}";
    }
}
