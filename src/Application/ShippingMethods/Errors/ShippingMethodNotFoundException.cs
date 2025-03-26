using SharedKernel.Errors;

namespace Application.ShippingMethods.Errors;

/// <summary>
/// Exception thrown when a required shipping method was not found.
/// </summary>
public class ShippingMethodNotFoundException : BaseException
{
    private const string DefaultTitle = "Shipping Method Not Found";
    private const string DefaultMessage =
        "The shipping method being queried was not found";
    private static readonly ErrorCode _defaultErrorCode = ErrorCode.NotFound;

    internal ShippingMethodNotFoundException()
        : base(DefaultMessage, DefaultTitle, _defaultErrorCode)
    {
    }

    internal ShippingMethodNotFoundException(string message)
        : base(message, DefaultTitle, _defaultErrorCode)
    {
    }

    internal ShippingMethodNotFoundException(string message, Exception innerException)
        : base(message, DefaultTitle, _defaultErrorCode, innerException)
    {
    }
}
