namespace IntegrationTests.TestUtils.Constants;

public static partial class TestConstants
{
    /// <summary>
    /// Defines the endpoints for the user resources.
    /// </summary>
    public static class UserEndpoints
    {
        private const string BaseEndpoint = "/users";
        private static string ResourceWithIdentifier(string id) => $"{BaseEndpoint}/{id}";

        /// <summary>
        /// Endpoint to deactivate a user.
        /// </summary>
        public static string DeactivateUser(string id) => ResourceWithIdentifier(id);
        /// <summary>
        /// Endpoint to get all users.
        /// </summary>
        public const string GetAllUsers = BaseEndpoint;
        /// <summary>
        /// Endpoint to get a user by authentication token.
        /// </summary>
        public const string GetUserByAuthenticationToken = $"{BaseEndpoint}/self";
        /// <summary>
        /// Endpoint to get a user by id.
        /// </summary>
        public static string GetUserById(string id) => ResourceWithIdentifier(id);
        /// <summary>
        /// Endpoint to update a user.
        /// </summary>
        public static string UpdateUser(string id) => ResourceWithIdentifier(id);
    }
}
