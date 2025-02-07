namespace IntegrationTests.TestUtils.Constants;

public static partial class TestConstants
{
    /// <summary>
    /// Defines the endpoints for the authentication resources.
    /// </summary>
    public static class AuthenticationEndpoints
    {
        /// <summary>
        /// Endpoint to register a customer.
        /// </summary>
        public const string RegisterCustomer = "/auth/register/users/customers";
        /// <summary>
        /// Endpoint to log in an user.
        /// </summary>
        public const string LoginUser = "/auth/login/users";
        /// <summary>
        /// Endpoint to log in a carrier.
        /// </summary>
        public const string LoginCarrier = "/auth/login/carriers";
    }
}
