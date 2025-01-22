using Application.Common.Security.Authorization.Requests;
using Application.Common.Security.Authorization.Roles;
using Application.Common.Security.Authorization;

namespace Application.UnitTests.Common.Behaviors.TestUtils;

/// <summary>
/// Utilities to the test authorization behavior.
/// </summary>
public static class AuthorizationBehaviorUtils
{
    /// <summary>
    /// Represents a response.
    /// </summary>
    /// <param name="Message">The response message.</param>
    public record TestResponse(string Message);

    /// <summary>
    /// Represents a request without authorization attributes.
    /// </summary>
    public class TestRequest : IRequestWithAuthorization<TestResponse>
    {
        /// <inheritdoc/>
        public string? UserId => null;
    }

    /// <summary>
    /// Represents a request with one authorization attribute.
    /// </summary>
    [Authorize(roleName: nameof(Role.Admin))]
    public class TestRequestWithAuth : TestRequest;
}
