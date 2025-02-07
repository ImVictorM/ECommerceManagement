using SharedKernel.Errors;

namespace Application.Shipments.Errors;

/// <summary>
/// Exception thrown when a required shipment was not found.
/// </summary>
public class ShipmentNotFoundException : BaseException
{
    private const string DefaultTitle = "Shipment Not Found";
    private const string DefaultMessage = "The shipment being queried was not found";
    private static readonly ErrorCode _defaultErrorCode = ErrorCode.NotFound;

    internal ShipmentNotFoundException() : base(DefaultMessage, DefaultTitle, _defaultErrorCode)
    {
    }

    internal ShipmentNotFoundException(string message) : base(message, DefaultTitle, _defaultErrorCode)
    {
    }

    internal ShipmentNotFoundException(string message, Exception innerException) : base(message, DefaultTitle, _defaultErrorCode, innerException)
    {
    }
}
