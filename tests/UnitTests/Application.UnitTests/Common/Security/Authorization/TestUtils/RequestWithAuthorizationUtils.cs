using Application.Common.Security.Authorization.Requests;

namespace Application.UnitTests.Common.Security.Authorization.TestUtils;

/// <summary>
/// Utilities that defines requests with authorization for testing.
/// </summary>
public static class RequestWithAuthorizationUtils
{
    /// <summary>
    /// Represents a response.
    /// </summary>
    /// <param name="Message">The response message.</param>
    public record TestResponse(string Message);

    /// <summary>
    /// Represents a request with empty user id.
    /// </summary>
    public class TestRequest() : IRequestWithAuthorization<TestResponse>;

    /// <summary>
    /// Represents a request with defined user id.
    /// </summary>
    /// <param name="UserId">The user id.</param>
    public record TestRequestWithUser(string UserId) : IRequestWithAuthorization<TestResponse>, IUserSpecificResource;

}
