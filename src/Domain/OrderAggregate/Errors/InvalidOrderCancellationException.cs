using SharedKernel.Errors;

namespace Domain.OrderAggregate.Errors;

/// <summary>
/// Exception thrown when an order cannot be canceled because of its current state.
/// </summary>
public class InvalidOrderCancellationException : BaseException
{
    private const string DefaultTitle = "Invalid Order Cancellation";
    private const string DefaultMessage =
        "Only orders with a 'Pending' status can be canceled";

    private static readonly ErrorCode _defaultErrorCode = ErrorCode.InvalidOperation;

    internal InvalidOrderCancellationException()
        : base(DefaultMessage, DefaultTitle, _defaultErrorCode)
    {
    }

    internal InvalidOrderCancellationException(string message)
        : base(message, DefaultTitle, _defaultErrorCode)
    {
    }

    internal InvalidOrderCancellationException(
        string message,
        Exception innerException
    )
        : base(message, DefaultTitle, _defaultErrorCode, innerException)
    {
    }
}
