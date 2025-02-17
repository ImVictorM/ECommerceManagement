using Domain.ShippingMethodAggregate.ValueObjects;

using SharedKernel.Models;

namespace Domain.ShippingMethodAggregate;

/// <summary>
/// Represents a shipping method.
/// </summary>
public class ShippingMethod : AggregateRoot<ShippingMethodId>
{
    /// <summary>
    /// Gets the shipping method name.
    /// </summary>
    public string Name { get; private set; } = null!;
    /// <summary>
    /// Gets the amount charged for the shipping.
    /// </summary>
    public decimal Price { get; private set; }
    /// <summary>
    /// Gets the static estimated delivery days.
    /// </summary>
    public int EstimatedDeliveryDays { get; private set; }

    private ShippingMethod() { }

    private ShippingMethod(
        string name,
        decimal price,
        int estimatedDeliveryDays
    )
    {
        Name = name;
        Price = price;
        EstimatedDeliveryDays = estimatedDeliveryDays;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ShippingMethod"/> class.
    /// </summary>
    /// <param name="name">The shipping method name.</param>
    /// <param name="price">The amount charged for the shipping method.</param>
    /// <param name="estimatedDeliveryDays">A static estimated delivery days.</param>
    /// <returns>A new instance of the <see cref="ShippingMethod"/> class.</returns>
    public static ShippingMethod Create(
        string name,
        decimal price,
        int estimatedDeliveryDays
    )
    {
        return new ShippingMethod(name, price, estimatedDeliveryDays);
    }

    /// <summary>
    /// Partially or fully updates the shipping method.
    /// </summary>
    /// <param name="name">The new shipping method name.</param>
    /// <param name="price">The new shipping method price.</param>
    /// <param name="estimatedDeliveryDays">The new shipping method estimated delivery days.</param>
    public void Update(
        string? name = null,
        decimal? price = null,
        int? estimatedDeliveryDays = null
    )
    {
        Name = name ?? Name;
        Price = price ?? Price;
        EstimatedDeliveryDays = estimatedDeliveryDays ?? EstimatedDeliveryDays;
    }
}
