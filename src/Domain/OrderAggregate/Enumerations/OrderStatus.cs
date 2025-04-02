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
    public static readonly OrderStatus Pending = new(1, nameof(Pending));
    /// <summary>
    /// Represents a paid status.
    /// </summary>
    public static readonly OrderStatus Paid = new(2, nameof(Paid));
    /// <summary>
    /// Represents a shipped status.
    /// </summary>
    public static readonly OrderStatus Shipped = new(3, nameof(Shipped));
    /// <summary>
    /// Represents a delivered status.
    /// </summary>
    public static readonly OrderStatus Delivered = new(4, nameof(Delivered));
    /// <summary>
    /// Represents a canceled status.
    /// </summary>
    public static readonly OrderStatus Canceled = new(5, nameof(Canceled));

    private OrderStatus() { }

    private OrderStatus(long id, string name) : base(id, name)
    {
    }

    /// <summary>
    /// Lists all of the defined order status.
    /// </summary>
    /// <returns>
    /// A list containing all the defined <see cref="OrderStatus"/>.
    /// </returns>
    public static IReadOnlyList<OrderStatus> List()
    {
        return GetAll<OrderStatus>().ToList();
    }
}
