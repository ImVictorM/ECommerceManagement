using Domain.Common.Errors;
using Domain.Common.Models;

namespace Domain.OrderAggregate.ValueObjects;

/// <summary>
/// Represents an order status.
/// </summary>
public sealed class OrderStatus : ValueObject
{
    /// <summary>
    /// Represents a pending status.
    /// </summary>
    public static readonly OrderStatus Pending = new(nameof(Pending).ToLowerInvariant());
    /// <summary>
    /// Represents a paid status.
    /// </summary>
    public static readonly OrderStatus Paid = new(nameof(Paid).ToLowerInvariant());
    /// <summary>
    /// Represents a shipped status.
    /// </summary>
    public static readonly OrderStatus Shipped = new(nameof(Shipped).ToLowerInvariant());
    /// <summary>
    /// Represents a delivered status.
    /// </summary>
    public static readonly OrderStatus Delivered = new(nameof(Delivered).ToLowerInvariant());
    /// <summary>
    /// Represents a canceled status.
    /// </summary>
    public static readonly OrderStatus Canceled = new(nameof(Canceled).ToLowerInvariant());

    /// <summary>
    /// Gets the order status name.
    /// </summary>
    public string Name { get; } = string.Empty;

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderStatus"/> class.
    /// </summary>
    private OrderStatus() { }

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderStatus"/> class.
    /// </summary>
    /// <param name="name">The order status name.</param>
    private OrderStatus(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="OrderStatus"/> class.
    /// </summary>
    /// <param name="name">The order status name.</param>
    /// <returns>A new instance of the <see cref="OrderStatus"/> class.</returns>
    public static OrderStatus Create(string name)
    {
        if (GetOrderStatusByName(name) == null) throw new DomainValidationException($"The {name} order status does not exist");

        return new OrderStatus(name);
    }

    /// <summary>
    /// Gets all the order statuses in a list format.
    /// </summary>
    /// <returns>All the order statuses.</returns>
    public static IEnumerable<OrderStatus> List()
    {
        return
        [
            Pending,
            Paid,
            Shipped,
            Delivered,
            Canceled
        ];
    }

    /// <summary>
    /// Gets an order status by name, or null if not found.
    /// </summary>
    /// <param name="name">The order status name.</param>
    /// <returns>The order status or null.</returns>
    private static OrderStatus? GetOrderStatusByName(string name)
    {
        return List().FirstOrDefault(orderStatus => orderStatus.Name == name);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Name;
    }
}
