using SharedKernel.Extensions;
using SharedKernel.Models;

namespace Domain.OrderAggregate.Enumerations;

/// <summary>
/// Represents an order status.
/// </summary>
public sealed class OrderStatus : BaseEnumeration
{
    /// <summary>
    /// Represents a pending status.
    /// </summary>
    public static readonly OrderStatus Pending = new(1, nameof(Pending).ToLowerSnakeCase());
    /// <summary>
    /// Represents a paid status.
    /// </summary>
    public static readonly OrderStatus Paid = new(2, nameof(Paid).ToLowerSnakeCase());
    /// <summary>
    /// Represents a shipped status.
    /// </summary>
    public static readonly OrderStatus Shipped = new(3, nameof(Shipped).ToLowerSnakeCase());
    /// <summary>
    /// Represents a delivered status.
    /// </summary>
    public static readonly OrderStatus Delivered = new(4, nameof(Delivered).ToLowerSnakeCase());
    /// <summary>
    /// Represents a canceled status.
    /// </summary>
    public static readonly OrderStatus Canceled = new(5, nameof(Canceled).ToLowerSnakeCase());

    private OrderStatus() { }

    private OrderStatus(long id, string name) : base(id, name)
    {
    }

    /// <summary>
    /// Retrieves all the order status.
    /// </summary>
    /// <returns>An enumerable containing all order statuses.</returns>
    public static IEnumerable<OrderStatus> List()
    {
        return GetAll<OrderStatus>();
    }
}
