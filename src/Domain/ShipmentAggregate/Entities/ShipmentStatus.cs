using Domain.Common.Errors;
using Domain.Common.Models;
using Domain.ShipmentAggregate.ValueObjects;
using SharedResources.Extensions;

namespace Domain.ShipmentAggregate.Entities;

/// <summary>
/// Represents the shipment status.
/// </summary>
public sealed class ShipmentStatus : Entity<ShipmentStatusId>
{
    /// <summary>
    /// Represents the pending status.
    /// </summary>
    public static readonly ShipmentStatus Pending = new(ShipmentStatusId.Create(1), nameof(Pending).ToLowerSnakeCase());
    /// <summary>
    /// Represents the shipped status.
    /// </summary>
    public static readonly ShipmentStatus Shipped = new(ShipmentStatusId.Create(2), nameof(Shipped).ToLowerSnakeCase());
    /// <summary>
    /// Represents the in route status.
    /// </summary>
    public static readonly ShipmentStatus InRoute = new(ShipmentStatusId.Create(3), nameof(InRoute).ToLowerSnakeCase());
    /// <summary>
    /// Represents the delivered status.
    /// </summary>
    public static readonly ShipmentStatus Delivered = new(ShipmentStatusId.Create(4), nameof(Delivered).ToLowerSnakeCase());
    /// <summary>
    /// Represents the canceled status.
    /// </summary>
    public static readonly ShipmentStatus Canceled = new(ShipmentStatusId.Create(5), nameof(Canceled).ToLowerSnakeCase());

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
    /// <param name="id">The shipment status identifier.</param>
    /// <param name="name">The shipment status name.</param>
    private ShipmentStatus(ShipmentStatusId id, string name) : base(id)
    {
        Name = name;

        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ShipmentStatus"/> class.
    /// </summary>
    /// <param name="name">The shipment status name.</param>
    /// <returns>A new instance of the <see cref="ShipmentStatus"/> class.</returns>
    public static ShipmentStatus Create(string name)
    {
        return GetShipmentStatusByName(name) ?? throw new DomainValidationException($"The {name} shipment status does not exist");
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
}
