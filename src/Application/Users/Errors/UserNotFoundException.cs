using SharedKernel.Errors;

namespace Application.Users.Errors;

/// <summary>
/// The exception that is thrown when a user being retrieved does not exist.
/// </summary>
public sealed class UserNotFoundException : BaseException
{
    private const string DefaultTitle = "User Not Found";
    private const string DefaultMessage = "The user being queried was not found";
    private static readonly ErrorCode _defaultErrorCode = ErrorCode.NotFound;

    internal UserNotFoundException() : base(DefaultMessage, DefaultTitle, _defaultErrorCode)
    {
    }

    internal UserNotFoundException(string message) : base(message, DefaultTitle, _defaultErrorCode)
    {
    }

    internal UserNotFoundException(string message, Exception innerException) : base(message, DefaultTitle, _defaultErrorCode, innerException)
    {
    }
}
