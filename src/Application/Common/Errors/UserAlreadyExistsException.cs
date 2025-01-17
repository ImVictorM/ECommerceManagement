using SharedKernel.Errors;

namespace Application.Common.Errors;

/// <summary>
/// The exception that is thrown when a user that should not exist already exists in certain context.
/// </summary>
public sealed class UserAlreadyExistsException : BaseException
{
    private const string DefaultTitle = "User Conflict";
    private const string DefaultMessage = "The user already exists";
    private static readonly ErrorCode _defaultErrorCode = ErrorCode.Conflict;

    internal UserAlreadyExistsException() : base(DefaultMessage, DefaultTitle, _defaultErrorCode)
    {
    }

    internal UserAlreadyExistsException(string message) : base(message, DefaultTitle, _defaultErrorCode)
    {
    }

    internal UserAlreadyExistsException(string message, Exception innerException) : base(message, DefaultTitle, _defaultErrorCode, innerException)
    {
    }
}
