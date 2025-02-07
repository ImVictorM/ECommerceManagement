namespace IntegrationTests.TestUtils.Constants;

public static partial class TestConstants
{
    /// <summary>
    /// Defines the endpoints for the category resources.
    /// </summary>
    public static class CategoryEndpoints
    {
        private const string BaseEndpoint = "/products/categories";
        private static string ResourceWithIdentifier(string id) => $"{BaseEndpoint}/{id}";

        /// <summary>
        /// Endpoint to create a category.
        /// </summary>
        public const string CreateCategory = BaseEndpoint;
        /// <summary>
        /// Endpoint to delete a category.
        /// </summary>
        public static string DeleteCategory(string id) => ResourceWithIdentifier(id);
        /// <summary>
        /// Endpoint to get all categories.
        /// </summary>
        public const string GetCategories = BaseEndpoint;
        /// <summary>
        /// Endpoint to get a category by id.
        /// </summary>
        public static string GetCategoryById(string id) => ResourceWithIdentifier(id);
        /// <summary>
        /// Endpoint to update a category.
        /// </summary>
        public static string UpdateCategory(string id) => ResourceWithIdentifier(id);
    }
}
