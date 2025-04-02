using MediatR;

namespace Application.UnitTests.Common.Validation.TestUtils;

/// <summary>
/// Utilities to test the validation behavior.
/// </summary>
public static class ValidationBehaviorUtils
{
    /// <summary>
    /// Represents a response.
    /// </summary>
    /// <param name="Message">The response message.</param>
    public record TestResponse(string Message);

    /// <summary>
    /// Represents a request.
    /// </summary>
    /// <param name="Data">The request data.</param>
    public record TestRequest(string Data) : IRequest<TestResponse>;
}
