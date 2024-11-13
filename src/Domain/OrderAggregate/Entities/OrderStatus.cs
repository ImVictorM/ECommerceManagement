using Domain.OrderAggregate.ValueObjects;
using SharedKernel.Errors;
using SharedKernel.Extensions;
using SharedKernel.Models;

namespace Domain.OrderAggregate.Entities;

/// <summary>
/// Represents an order status.
/// </summary>
public sealed class OrderStatus : Entity<OrderStatusId>
{
    /// <summary>
    /// Represents a pending status.
    /// </summary>
    public static readonly OrderStatus Pending = new(OrderStatusId.Create(1), nameof(Pending).ToLowerSnakeCase());
    /// <summary>
    /// Represents a paid status.
    /// </summary>
    public static readonly OrderStatus Paid = new(OrderStatusId.Create(2), nameof(Paid).ToLowerSnakeCase());
    /// <summary>
    /// Represents a shipped status.
    /// </summary>
    public static readonly OrderStatus Shipped = new(OrderStatusId.Create(3), nameof(Shipped).ToLowerSnakeCase());
    /// <summary>
    /// Represents a delivered status.
    /// </summary>
    public static readonly OrderStatus Delivered = new(OrderStatusId.Create(4), nameof(Delivered).ToLowerSnakeCase());
    /// <summary>
    /// Represents a canceled status.
    /// </summary>
    public static readonly OrderStatus Canceled = new(OrderStatusId.Create(5), nameof(Canceled).ToLowerSnakeCase());

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
    /// <param name="id">The order status identifier.</param>
    /// <param name="name">The order status name.</param>
    private OrderStatus(OrderStatusId id, string name) : base(id)
    {
        Name = name;

        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="OrderStatus"/> class.
    /// </summary>
    /// <param name="name">The order status name.</param>
    /// <returns>A new instance of the <see cref="OrderStatus"/> class.</returns>
    public static OrderStatus Create(string name)
    {
        return GetOrderStatusByName(name) ?? throw new DomainValidationException($"The {name} order status does not exist");
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
}
