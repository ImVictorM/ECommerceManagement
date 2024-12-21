using Domain.CouponAggregate.ValueObjects;
using SharedKernel.Interfaces;
using SharedKernel.Models;
using SharedKernel.ValueObjects;

namespace Domain.CouponAggregate;

/// <summary>
/// Represents a coupon.
/// </summary>
public class Coupon : AggregateRoot<CouponId>, IActivatable
{
    /// <summary>
    /// Gets the coupon discount.
    /// </summary>
    public Discount Discount { get; private set; } = null!;

    /// <summary>
    /// Gets the coupon code.
    /// </summary>
    public string Code { get; private set; } = null!;

    /// <summary>
    /// Gets the coupon usage limit.
    /// </summary>
    public int UsageLimit { get; private set; }

    /// <summary>
    /// Indicates if the coupon should auto apply.
    /// </summary>
    public bool AutoApply { get; private set; }

    /// <summary>
    /// Gets the minimal price to apply the coupon.
    /// </summary>
    public decimal MinPrice { get; private set; }

    /// <summary>
    /// Indicates if the coupon is active.
    /// </summary>
    public bool IsActive { get; set; }

    private Coupon() { }

    private Coupon(
        Discount discount,
        string code,
        int usageLimit,
        decimal minPrice,
        bool autoApply
    )
    {
        Discount = discount;
        Code = code;
        UsageLimit = usageLimit;
        AutoApply = autoApply;
        MinPrice = minPrice;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Coupon"/> class.
    /// </summary>
    /// <param name="discount">The coupon discount.</param>
    /// <param name="code">The coupon code.</param>
    /// <param name="usageLimit">The coupon usage limit.</param>
    /// <param name="minPrice">The minimum price to apply the coupon.</param>
    /// <param name="autoApply">Indicates if the coupon should auto apply.</param>
    /// <returns>A new instance of the <see cref="Coupon"/> class.</returns>
    public static Coupon Create(
        Discount discount,
        string code,
        int usageLimit,
        decimal minPrice = 0m,
        bool autoApply = false
    )
    {
        return new Coupon(discount, code, usageLimit, minPrice, autoApply);
    }

    /// <summary>
    /// Deactivates the coupon.
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
    }
}
