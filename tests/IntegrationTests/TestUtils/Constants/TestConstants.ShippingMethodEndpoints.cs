namespace IntegrationTests.TestUtils.Constants;

public static partial class TestConstants
{
    /// <summary>
    /// Defines the endpoints for the shipping method resources.
    /// </summary>
    public static class ShippingMethodEndpoints
    {
        private const string BaseEndpoint = "/shipping/methods";
        private static string ResourceWithIdentifier(string id) => $"{BaseEndpoint}/{id}";

        /// <summary>
        /// Endpoint to create a shipping method.
        /// </summary>
        public const string CreateShippingMethod = BaseEndpoint;
        /// <summary>
        /// Endpoint to delete a shipping method.
        /// </summary>
        public static string DeleteShippingMethod(string id) => ResourceWithIdentifier(id);
        /// <summary>
        /// Endpoint to get all shipping methods.
        /// </summary>
        public const string GetShippingMethods = BaseEndpoint;
        /// <summary>
        /// Endpoint to get a shipping method by id.
        /// </summary>
        public static string GetShippingMethodById(string id) => ResourceWithIdentifier(id);
        /// <summary>
        /// Endpoint to update a shipping method.
        /// </summary>
        public static string UpdateShippingMethod(string id) => ResourceWithIdentifier(id);
    }
}
