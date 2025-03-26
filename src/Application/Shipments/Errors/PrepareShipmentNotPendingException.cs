using Domain.ShipmentAggregate.Enumerations;

using SharedKernel.Errors;

namespace Application.Shipments.Errors;

/// <summary>
/// Exception thrown when attempting to prepare a shipment that is not
/// in the 'Pending' status.
/// </summary>
public class PrepareShipmentNotPendingException : BaseException
{
    private const string DefaultTitle = "Invalid Shipment Status";
    private const string DefaultMessage =
        "The shipment cannot be prepared because it is " +
        "not in the 'Pending' status";
    private static readonly ErrorCode _defaultErrorCode = ErrorCode.InternalError;

    internal PrepareShipmentNotPendingException()
        : base(DefaultMessage, DefaultTitle, _defaultErrorCode)
    {
    }

    internal PrepareShipmentNotPendingException(string message)
        : base(message, DefaultTitle, _defaultErrorCode)
    {
    }

    internal PrepareShipmentNotPendingException(string message, Exception innerException)
        : base(message, DefaultTitle, _defaultErrorCode, innerException)
    {
    }

    internal PrepareShipmentNotPendingException(ShipmentStatus currentStatus)
        : base(
            $"Shipment status was expected to be 'Pending'" +
            $" but was '{currentStatus.Name}' instead",
            DefaultTitle,
            _defaultErrorCode
        )
    {
    }
}
