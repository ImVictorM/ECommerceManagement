using SharedKernel.Errors;

namespace Application.Orders.Errors;

/// <summary>
/// Exception thrown when the order shipping method is invalid.
/// </summary>
public class InvalidShippingMethodException : BaseException
{
    private const string DefaultTitle = "Invalid Shipping Method";
    private const string DefaultMessage = "The selected shipping method does not exist";
    private static readonly ErrorCode _defaultErrorCode = ErrorCode.ValidationError;

    internal InvalidShippingMethodException() : base(DefaultMessage, DefaultTitle, _defaultErrorCode)
    {
    }

    internal InvalidShippingMethodException(string message) : base(message, DefaultTitle, _defaultErrorCode)
    {
    }

    internal InvalidShippingMethodException(string message, Exception innerException) : base(message, DefaultTitle, _defaultErrorCode, innerException)
    {
    }
}
