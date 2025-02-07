namespace Application.Common.Security.Authorization.Requests;

/// <summary>
/// Represents a shipment specific resource request.
/// </summary>
public interface IShipmentSpecificResource
{
    /// <summary>
    /// Gets the shipment id.
    /// </summary>
    string ShipmentId { get; }
}
