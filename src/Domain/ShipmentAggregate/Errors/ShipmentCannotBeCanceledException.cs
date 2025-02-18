using SharedKernel.Errors;

namespace Domain.ShipmentAggregate.Errors;

/// <summary>
/// Exception thrown when attempting to cancel a shipment that is not in a cancellable state.
/// </summary>
public class ShipmentCannotBeCanceledException : BaseException
{
    private const string DefaultTitle = "Shipment Cannot Be Canceled";
    private const string DefaultMessage = "The shipment cannot be canceled because it is no longer pending";

    private static readonly ErrorCode _defaultErrorCode = ErrorCode.InvalidOperation;

    internal ShipmentCannotBeCanceledException() : base(DefaultMessage, DefaultTitle, _defaultErrorCode)
    {
    }

    internal ShipmentCannotBeCanceledException(string message)
        : base(message, DefaultTitle, _defaultErrorCode)
    {
    }

    internal ShipmentCannotBeCanceledException(string message, Exception innerException)
        : base(message, DefaultTitle, _defaultErrorCode, innerException)
    {
    }
}
