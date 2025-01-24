using SharedKernel.Errors;

namespace Application.Common.Errors;

/// <summary>
/// Exception thrown when a user tries to access a resource they are not allowed.
/// </summary>
public class NotAllowedException : BaseException
{
    private const string DefaultTitle = "Authorization Failed";
    private const string DefaultMessage = "The current user does not have the required privileges to access the resource";
    private static readonly ErrorCode _defaultErrorCode = ErrorCode.NotAllowed;

    internal NotAllowedException() : base(DefaultMessage, DefaultTitle, _defaultErrorCode)
    {
    }

    internal NotAllowedException(string message) : base(message, DefaultTitle, _defaultErrorCode)
    {
    }

    internal NotAllowedException(string message, Exception innerException)
        : base(message, DefaultTitle, _defaultErrorCode, innerException)
    {
    }
}
