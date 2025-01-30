using SharedKernel.Errors;

namespace Application.Authentication.Errors;

/// <summary>
/// The exception that is thrown when an authentication attempt fails.
/// </summary>
public sealed class AuthenticationFailedException : BaseException
{
    private const string DefaultTitle = "Authentication Failed";
    private const string DefaultMessage = "User email or password is incorrect";
    private static readonly ErrorCode _defaultErrorCode = ErrorCode.InvalidOperation;

    internal AuthenticationFailedException() : base(DefaultMessage, DefaultTitle, _defaultErrorCode)
    {
    }

    internal AuthenticationFailedException(string message) : base(message, DefaultTitle, _defaultErrorCode)
    {
    }

    internal AuthenticationFailedException(string message, Exception innerException) : base(message, DefaultTitle, _defaultErrorCode, innerException)
    {
    }
}
