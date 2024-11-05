using System.Net;
using SharedKernel.Errors;

namespace WebApi.Common.Extensions;

/// <summary>
/// Extension methods for the <see cref="ErrorCode"/> class.
/// </summary>
public static class ErrorCodeExtensions
{
    private static readonly Dictionary<ErrorCode, HttpStatusCode> _errorCodeToHttpStatusCode = new()
    {
        { ErrorCode.NotFound, HttpStatusCode.NotFound },
        { ErrorCode.Conflict, HttpStatusCode.Conflict },
        { ErrorCode.InvalidOperation, HttpStatusCode.BadRequest },
        { ErrorCode.InternalError, HttpStatusCode.InternalServerError },
        { ErrorCode.ValidationError, HttpStatusCode.BadRequest }
    };

    /// <summary>
    /// Parses the error code to an HTTP status code.
    /// </summary>
    /// <param name="errorCode">The current error code.</param>
    /// <returns>An HTTP status code.</returns>
    public static HttpStatusCode ToHttpStatusCode(this ErrorCode errorCode)
    {
        return _errorCodeToHttpStatusCode.TryGetValue(errorCode, out var statusCode) ? statusCode : HttpStatusCode.InternalServerError;

    }
}
