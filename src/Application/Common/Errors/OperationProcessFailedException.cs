using SharedKernel.Errors;

namespace Application.Common.Errors;

/// <summary>
/// Exception thrown when some important operation process fails.
/// </summary>
public class OperationProcessFailedException : BaseException
{
    private const string DefaultTitle = "Operation Processing Failed";
    private const string DefaultMessage = "A critical error occurred while processing the request";
    private static readonly ErrorCode _defaultErrorCode = ErrorCode.InternalError;

    internal OperationProcessFailedException() : base(DefaultMessage, DefaultTitle, _defaultErrorCode)
    {
    }

    internal OperationProcessFailedException(string message) : base(message, DefaultTitle, _defaultErrorCode)
    {
    }

    internal OperationProcessFailedException(string message, Exception innerException)
        : base(message, DefaultTitle, _defaultErrorCode, innerException)
    {
    }
}
