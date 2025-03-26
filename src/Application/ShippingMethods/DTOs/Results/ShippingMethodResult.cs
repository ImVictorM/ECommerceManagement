using Domain.ShippingMethodAggregate;

namespace Application.ShippingMethods.DTOs.Results;

/// <summary>
/// Represents a shipping method result.
/// </summary>
public class ShippingMethodResult
{
    /// <summary>
    /// Gets the shipping method identifier.
    /// </summary>
    public string Id { get; }
    /// <summary>
    /// Gets the shipping method name.
    /// </summary>
    public string Name { get; }
    /// <summary>
    /// Gets the shipping method price.
    /// </summary>
    public decimal Price { get; }
    /// <summary>
    /// Gets the shipping method estimated delivery days.
    /// </summary>
    public int EstimatedDeliveryDays { get; }

    private ShippingMethodResult(ShippingMethod shippingMethod)
    {
        Id = shippingMethod.Id.ToString();
        Name = shippingMethod.Name;
        Price = shippingMethod.Price;
        EstimatedDeliveryDays = shippingMethod.EstimatedDeliveryDays;
    }

    internal static ShippingMethodResult FromShippingMethod(
        ShippingMethod shippingMethod
    )
    {
        return new ShippingMethodResult(shippingMethod);
    }
};
