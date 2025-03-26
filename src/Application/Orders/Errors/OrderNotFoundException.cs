using SharedKernel.Errors;

namespace Application.Orders.Errors;

/// <summary>
/// The exception that is thrown when the order being retrieved does not exist.
/// </summary>
public class OrderNotFoundException : BaseException
{
    private const string DefaultTitle = "Order Not Found";
    private const string DefaultMessage = "The order being queried was not found";
    private static readonly ErrorCode _defaultErrorCode = ErrorCode.NotFound;

    internal OrderNotFoundException()
        : base(DefaultMessage, DefaultTitle, _defaultErrorCode)
    {
    }

    internal OrderNotFoundException(string message)
        : base(message, DefaultTitle, _defaultErrorCode)
    {
    }

    internal OrderNotFoundException(string message, Exception innerException)
        : base(message, DefaultTitle, _defaultErrorCode, innerException)
    {
    }
}
