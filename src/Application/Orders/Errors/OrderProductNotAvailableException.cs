using SharedKernel.Errors;

namespace Application.Orders.Errors;

/// <summary>
/// Exception thrown when a product in an order is not available.
/// </summary>
public class OrderProductNotAvailableException : BaseException
{
    private const string DefaultTitle = "Product Not Available";
    private const string DefaultMessage = "The product you're trying to order is currently unavailable";
    private static readonly ErrorCode _defaultErrorCode = ErrorCode.ValidationError;

    internal OrderProductNotAvailableException() : base(DefaultMessage, DefaultTitle, _defaultErrorCode)
    {
    }

    internal OrderProductNotAvailableException(string message) : base(message, DefaultTitle, _defaultErrorCode)
    {
    }

    internal OrderProductNotAvailableException(string message, Exception innerException) : base(message, DefaultTitle, _defaultErrorCode, innerException)
    {
    }
}
