namespace IntegrationTests.TestUtils.Constants;

public static partial class TestConstants
{
    /// <summary>
    /// Defines the endpoints for the authentication resources.
    /// </summary>
    public static class AuthenticationEndpoints
    {
        /// <summary>
        /// The register customer endpoint.
        /// </summary>
        public const string RegisterCustomer = "/auth/register/users/customers";
        /// <summary>
        /// The login user endpoint.
        /// </summary>
        public const string LoginUser = "/auth/login/users";
    }
}
