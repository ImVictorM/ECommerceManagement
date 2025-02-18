using Domain.ShipmentAggregate.Errors;

using SharedKernel.Errors;
using SharedKernel.Models;

namespace Domain.ShipmentAggregate.Enumerations;

/// <summary>
/// Represents the shipment status.
/// </summary>
public sealed class ShipmentStatus : BaseEnumeration
{
    /// <summary>
    /// Represents a canceled shipment status.
    /// </summary>
    public static readonly ShipmentStatus Canceled = new(-1, nameof(Canceled));
    /// <summary>
    /// Represents a pending shipment status.
    /// </summary>
    public static readonly ShipmentStatus Pending = new(1, nameof(Pending));
    /// <summary>
    /// Represents a preparing shipment status.
    /// </summary>
    public static readonly ShipmentStatus Preparing = new(2, nameof(Preparing));
    /// <summary>
    /// Represents a shipped shipment status.
    /// </summary>
    public static readonly ShipmentStatus Shipped = new(3, nameof(Shipped));
    /// <summary>
    /// Represents an in-route shipment status.
    /// </summary>
    public static readonly ShipmentStatus InRoute = new(4, nameof(InRoute));
    /// <summary>
    /// Represents a delivered shipment status.
    /// </summary>
    public static readonly ShipmentStatus Delivered = new(5, nameof(Delivered));

    private ShipmentStatus() { }

    private ShipmentStatus(long id, string name) : base(id, name)
    {
    }

    /// <summary>
    /// Retrieves the first shipment status.
    /// </summary>
    /// <returns>The first shipment status.</returns>
    public static ShipmentStatus First()
    {
        return Pending;
    }

    /// <summary>
    /// Advances the current status to the next.
    /// </summary>
    /// <returns>The next shipment status.</returns>
    /// <exception cref="OutOfRangeException">Thrown when the next status does not exist.</exception>
    /// <exception cref="ShipmentAlreadyCanceledException">Thrown when trying to advance a canceled shipment status.</exception>
    public ShipmentStatus Advance()
    {
        if (this == Canceled)
        {
            throw new ShipmentAlreadyCanceledException();
        }

        var statuses = GetAll<ShipmentStatus>().OrderBy(s => s.Id).ToList();

        var currentIndex = statuses.FindIndex(s => s == this);

        if (currentIndex < 0 || currentIndex + 1 >= statuses.Count)
        {
            throw new OutOfRangeException($"Cannot advance shipment status from {Name}. No next status exists.");
        }

        return statuses[currentIndex + 1];
    }

    /// <summary>
    /// Gets all the predefined shipment statuses in a list format.
    /// </summary>
    /// <returns>All predefined shipment statuses.</returns>
    public static IReadOnlyList<ShipmentStatus> List()
    {
        return GetAll<ShipmentStatus>().ToList();
    }
}
