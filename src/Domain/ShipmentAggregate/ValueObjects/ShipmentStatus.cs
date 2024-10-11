using Domain.Common.Errors;
using Domain.Common.Models;

namespace Domain.ShipmentAggregate.ValueObjects;

/// <summary>
/// Represents the shipment status.
/// </summary>
public sealed class ShipmentStatus : ValueObject
{
    /// <summary>
    /// Represents the pending status.
    /// </summary>
    public static readonly ShipmentStatus Pending = new(nameof(Pending).ToLowerInvariant());
    /// <summary>
    /// Represents the shipped status.
    /// </summary>
    public static readonly ShipmentStatus Shipped = new(nameof(Shipped).ToLowerInvariant());
    /// <summary>
    /// Represents the in route status.
    /// </summary>
    public static readonly ShipmentStatus InRoute = new("in_route");
    /// <summary>
    /// Represents the delivered status.
    /// </summary>
    public static readonly ShipmentStatus Delivered = new(nameof(Delivered).ToLowerInvariant());
    /// <summary>
    /// Represents the canceled status.
    /// </summary>
    public static readonly ShipmentStatus Canceled = new(nameof(Canceled).ToLowerInvariant());

    /// <summary>
    /// Gets the shipment status.
    /// </summary>
    public string Name { get; } = string.Empty;

    /// <summary>
    /// Initiates a new instance of the <see cref="ShipmentStatus"/> class.
    /// </summary>
    private ShipmentStatus() { }

    /// <summary>
    /// Initiates a new instance of the <see cref="ShipmentStatus"/> class.
    /// </summary>
    /// <param name="name">The shipment status name.</param>
    private ShipmentStatus(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ShipmentStatus"/> class.
    /// </summary>
    /// <param name="name">The shipment status name.</param>
    /// <returns>A new instance of the <see cref="ShipmentStatus"/> class.</returns>
    public static ShipmentStatus Create(string name)
    {
        if (GetShipmentStatusByName(name) == null) throw new DomainValidationException($"The {name} shipment status does not exist");

        return new ShipmentStatus(name);
    }

    /// <summary>
    /// Gets all the predefined shipment statuses in a list format.
    /// </summary>
    /// <returns>All predefined shipment statuses.</returns>
    public static IEnumerable<ShipmentStatus> List()
    {
        return [
            Pending,
            Shipped,
            InRoute,
            Delivered,
            Canceled
        ];
    }

    /// <summary>
    /// Gets a shipment status by name, or null if not found.
    /// </summary>
    /// <param name="name">The status name.</param>
    /// <returns>The shipment status or null.</returns>
    private static ShipmentStatus? GetShipmentStatusByName(string name)
    {
        return List().FirstOrDefault(status => status.Name == name);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Name;
    }
}
