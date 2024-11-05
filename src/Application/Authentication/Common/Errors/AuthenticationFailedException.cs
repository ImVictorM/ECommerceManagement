using SharedKernel.Errors;

namespace Application.Authentication.Common.Errors;

/// <summary>
/// The exception that is thrown when an authentication attempt fails.
/// </summary>
public sealed class AuthenticationFailedException : BaseException
{
    private const string DefaultTitle = "Authentication Failed";
    private static readonly ErrorCode _defaultErrorCode = ErrorCode.InvalidOperation;

    /// <summary>
    /// Initiates a new default instance of the <see cref="AuthenticationFailedException"/> class.
    /// </summary>
    public AuthenticationFailedException() : base("User email or password is incorrect", DefaultTitle, _defaultErrorCode)
    {
    }

    /// <summary>
    /// Initiates a new instance of the <see cref="AuthenticationFailedException"/> class with custom message.
    /// </summary>
    /// <param name="message">The exception message.</param>
    public AuthenticationFailedException(string message) : base(message, DefaultTitle, _defaultErrorCode)
    {
    }

    /// <summary>
    /// Initiates a new instance of the <see cref="AuthenticationFailedException"/> class with custom message and inner exception.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="innerException">The exception that caused the current exception.</param>
    public AuthenticationFailedException(string message, Exception innerException) : base(message, DefaultTitle, _defaultErrorCode, innerException)
    {
    }
}
