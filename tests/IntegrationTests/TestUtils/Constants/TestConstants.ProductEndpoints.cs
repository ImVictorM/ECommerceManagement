namespace IntegrationTests.TestUtils.Constants;

public static partial class TestConstants
{
    /// <summary>
    /// Defines the endpoints for the product resources.
    /// </summary>
    public static class ProductEndpoints
    {
        private const string BaseEndpoint = "/products";
        private static string ResourceWithIdentifier(string id) => $"{BaseEndpoint}/{id}";

        /// <summary>
        /// Endpoint to create a product.
        /// </summary>
        public const string CreateProduct = BaseEndpoint;
        /// <summary>
        /// Endpoint to deactivate a product.
        /// </summary>
        public static string DeactivateProduct(string id) => ResourceWithIdentifier(id);
        /// <summary>
        /// Endpoint to get all products.
        /// </summary>
        public const string GetAllProducts = BaseEndpoint;
        /// <summary>
        /// Endpoint to get a product by id.
        /// </summary>
        public static string GetProductById(string id) => ResourceWithIdentifier(id);
        /// <summary>
        /// Endpoint to update a product.
        /// </summary>
        public static string UpdateProductInventory(string id) => $"{ResourceWithIdentifier(id)}/inventory";
        /// <summary>
        /// Endpoint to update a product.
        /// </summary>
        public static string UpdateProduct(string id) => ResourceWithIdentifier(id);
    }
}
