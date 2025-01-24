using SharedKernel.Extensions;
using SharedKernel.Models;

namespace Domain.ShipmentAggregate.Enumerations;

/// <summary>
/// Represents the shipment status.
/// </summary>
public sealed class ShipmentStatus : BaseEnumeration
{
    /// <summary>
    /// Represents the pending status.
    /// </summary>
    public static readonly ShipmentStatus Pending = new(1, nameof(Pending).ToLowerSnakeCase());
    /// <summary>
    /// Represents the shipped status.
    /// </summary>
    public static readonly ShipmentStatus Shipped = new(2, nameof(Shipped).ToLowerSnakeCase());
    /// <summary>
    /// Represents the in route status.
    /// </summary>
    public static readonly ShipmentStatus InRoute = new(3, nameof(InRoute).ToLowerSnakeCase());
    /// <summary>
    /// Represents the delivered status.
    /// </summary>
    public static readonly ShipmentStatus Delivered = new(4, nameof(Delivered).ToLowerSnakeCase());
    /// <summary>
    /// Represents the canceled status.
    /// </summary>
    public static readonly ShipmentStatus Canceled = new(5, nameof(Canceled).ToLowerSnakeCase());

    private ShipmentStatus() { }

    private ShipmentStatus(long id, string name) : base(id, name)
    {
    }

    /// <summary>
    /// Gets all the predefined shipment statuses in a list format.
    /// </summary>
    /// <returns>All predefined shipment statuses.</returns>
    public static IEnumerable<ShipmentStatus> List()
    {
        return GetAll<ShipmentStatus>();
    }
}
