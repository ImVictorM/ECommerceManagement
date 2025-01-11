using SharedKernel.Errors;

namespace Domain.OrderAggregate.Errors;

/// <summary>
/// Exception thrown when marking the order as paid is not possible.
/// </summary>
public class InvalidOrderStateForPaymentException : BaseException
{
    private const string DefaultTitle = "Invalid Order State for Payment";
    private const string DefaultMessage = "The order must be in the 'Pending' status to be marked as paid";

    private static readonly ErrorCode _defaultErrorCode = ErrorCode.InvalidOperation;

    internal InvalidOrderStateForPaymentException() : base(DefaultMessage, DefaultTitle, _defaultErrorCode)
    {
    }

    internal InvalidOrderStateForPaymentException(string message) : base(message, DefaultTitle, _defaultErrorCode)
    {
    }

    internal InvalidOrderStateForPaymentException(string message, Exception innerException) : base(message, DefaultTitle, _defaultErrorCode, innerException)
    {
    }
}
