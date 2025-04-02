using SharedKernel.Errors;

namespace Application.Common.Errors;

/// <summary>
/// Exception thrown when an email conflict occurs.
/// </summary>
public class EmailConflictException : BaseException
{
    private const string DefaultTitle = "Email Conflict";
    private const string DefaultMessage = "The email you entered is already in use";
    private static readonly ErrorCode _defaultErrorCode = ErrorCode.Conflict;

    internal EmailConflictException()
        : base(DefaultMessage, DefaultTitle, _defaultErrorCode)
    {
    }

    internal EmailConflictException(string message)
        : base(message, DefaultTitle, _defaultErrorCode)
    {
    }

    internal EmailConflictException(string message, Exception innerException)
        : base(message, DefaultTitle, _defaultErrorCode, innerException)
    {
    }
}
