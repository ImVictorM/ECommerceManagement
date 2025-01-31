using SharedKernel.Errors;

namespace Domain.ShipmentAggregate.Errors;

/// <summary>
/// Exception thrown when attempting to perform an operation on a canceled shipment.
/// </summary>
public class ShipmentAlreadyCanceledException : BaseException
{
    private const string DefaultTitle = "Operation Not Allowed: Shipment Canceled";
    private const string DefaultMessage = "The operation cannot be completed because the shipment has already been canceled";

    private static readonly ErrorCode _defaultErrorCode = ErrorCode.InvalidOperation;

    internal ShipmentAlreadyCanceledException() : base(DefaultMessage, DefaultTitle, _defaultErrorCode)
    {
    }

    internal ShipmentAlreadyCanceledException(string message) : base(message, DefaultTitle, _defaultErrorCode)
    {
    }

    internal ShipmentAlreadyCanceledException(string message, Exception innerException) : base(message, DefaultTitle, _defaultErrorCode, innerException)
    {
    }
}
