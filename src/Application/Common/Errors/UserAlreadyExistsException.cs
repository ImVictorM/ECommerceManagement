using SharedKernel.Errors;

namespace Application.Common.Errors;

/// <summary>
/// The exception that is thrown when a user that should not exist already exists in certain context.
/// </summary>
public sealed class UserAlreadyExistsException : BaseException
{
    private const string DefaultTitle = "User Conflict";

    private static readonly ErrorCode _defaultErrorCode = ErrorCode.Conflict;

    /// <summary>
    /// Initiates a new default instance of the <see cref="UserAlreadyExistsException"/> class.
    /// </summary>
    public UserAlreadyExistsException() : base("The user already exists", DefaultTitle, _defaultErrorCode)
    {
    }

    /// <summary>
    /// Initiates a new instance of the <see cref="UserAlreadyExistsException"/> class with custom message.
    /// </summary>
    /// <param name="message">The exception message.</param>
    public UserAlreadyExistsException(string message) : base(message, DefaultTitle, _defaultErrorCode)
    {
    }

    /// <summary>
    /// Initiates a new instance of the <see cref="UserAlreadyExistsException"/> class with custom message and inner exception.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="innerException">The exception that caused the current exception.</param>
    public UserAlreadyExistsException(string message, Exception innerException) : base(message, DefaultTitle, _defaultErrorCode, innerException)
    {
    }
}
