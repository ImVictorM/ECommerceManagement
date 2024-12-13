using SharedKernel.Errors;

namespace Application.Users.Common.Errors;

/// <summary>
/// Exception thrown when a user tries to do something they are not allowed to do.
/// </summary>
public class UserNotAllowedException : BaseException
{
    private const string DefaultTitle = "User Not Allowed";
    private static readonly ErrorCode _defaultErrorCode = ErrorCode.NotAllowed;

    /// <summary>
    /// Initiates a new default instance of the <see cref="UserNotAllowedException"/> class.
    /// </summary>
    public UserNotAllowedException() : base("You do not have permission to perform this action", DefaultTitle, _defaultErrorCode)
    {
    }

    /// <summary>
    /// Initiates a new instance of the <see cref="UserNotAllowedException"/> class with custom message.
    /// </summary>
    /// <param name="message">The exception message.</param>
    public UserNotAllowedException(string message) : base(message, DefaultTitle, _defaultErrorCode)
    {
    }

    /// <summary>
    /// Initiates a new instance of the <see cref="UserNotAllowedException"/> class with custom message and inner exception.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="innerException">The exception that caused the current exception.</param>
    public UserNotAllowedException(string message, Exception innerException) : base(message, DefaultTitle, _defaultErrorCode, innerException)
    {
    }
}
