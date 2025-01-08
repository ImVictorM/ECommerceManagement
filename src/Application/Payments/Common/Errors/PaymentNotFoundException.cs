using SharedKernel.Errors;

namespace Application.Payments.Common.Errors;

/// <summary>
/// Exception thrown when the payment being retrieved was not found.
/// </summary>
public class PaymentNotFoundException : BaseException
{
    private const string DefaultTitle = "Payment Not Found";
    private const string DefaultMessage = "The payment being queried was not found";
    private static readonly ErrorCode _defaultErrorCode = ErrorCode.NotFound;

    internal PaymentNotFoundException() : base(DefaultMessage, DefaultTitle, _defaultErrorCode)
    {
    }

    internal PaymentNotFoundException(string message) : base(message, DefaultTitle, _defaultErrorCode)
    {
    }

    internal PaymentNotFoundException(string message, Exception innerException) : base(message, DefaultTitle, _defaultErrorCode, innerException)
    {
    }
}
