using SharedKernel.Errors;

namespace Application.Shipments.Errors;

/// <summary>
/// Exception thrown when trying to advance a pending shipment manually.
/// </summary>
public class AdvancePendingShipmentStatusException : BaseException
{
    private const string DefaultTitle = "Cannot Advance Shipment from Pending Status";

    private const string DefaultMessage = "The shipment cannot be advanced from the pending status. " +
        "It will automatically advance to 'Preparing' once the order payment is approved. " +
        "After reaching the 'Preparing' state, you can manually advance it further.";

    private static readonly ErrorCode _defaultErrorCode = ErrorCode.InvalidOperation;

    internal AdvancePendingShipmentStatusException() : base(DefaultMessage, DefaultTitle, _defaultErrorCode)
    {
    }

    internal AdvancePendingShipmentStatusException(string message) : base(message, DefaultTitle, _defaultErrorCode)
    {
    }

    internal AdvancePendingShipmentStatusException(string message, Exception innerException) : base(message, DefaultTitle, _defaultErrorCode, innerException)
    {
    }
}
