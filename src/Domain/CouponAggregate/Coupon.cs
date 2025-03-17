using Domain.CouponAggregate.Abstracts;
using Domain.CouponAggregate.ValueObjects;

using SharedKernel.Extensions;
using SharedKernel.Interfaces;
using SharedKernel.Models;
using SharedKernel.ValueObjects;

namespace Domain.CouponAggregate;

/// <summary>
/// Represents a coupon.
/// </summary>
public class Coupon : AggregateRoot<CouponId>, IToggleSwitch
{
    private const decimal DefaultMinimumPrice = 0m;
    private const bool DefaultAutoApply = false;

    private readonly HashSet<CouponRestriction> _restrictions = [];

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

    /// <inheritdoc/>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets the coupon restrictions.
    /// </summary>
    public IReadOnlySet<CouponRestriction> Restrictions => _restrictions;

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
        Code = code.ToUpperSnakeCase();
        UsageLimit = usageLimit;
        AutoApply = autoApply;
        MinPrice = minPrice;

        IsActive = true;
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
        decimal minPrice = DefaultMinimumPrice,
        bool autoApply = DefaultAutoApply
    )
    {
        return new Coupon(
            discount,
            code,
            usageLimit,
            minPrice,
            autoApply
        );
    }

    /// <summary>
    /// Updates the current coupon.
    /// </summary>
    /// <param name="discount">The new coupon discount.</param>
    /// <param name="code">The new coupon code.</param>
    /// <param name="usageLimit">The new coupon usage limit.</param>
    /// <param name="minPrice">The new coupon minimum price.</param>
    /// <param name="autoApply">
    /// A boolean value indicating if the coupon should auto apply.
    /// </param>
    public void Update(
        Discount discount,
        string code,
        int usageLimit,
        decimal minPrice = DefaultMinimumPrice,
        bool autoApply = DefaultAutoApply
    )
    {
        Discount = discount;
        Code = code.ToUpperSnakeCase();
        UsageLimit = usageLimit;
        AutoApply = autoApply;
        MinPrice = minPrice;
    }

    /// <summary>
    /// Assigns a restriction to the coupon.
    /// </summary>
    /// <param name="restriction">The restriction.</param>
    public void AssignRestriction(CouponRestriction restriction)
    {
        _restrictions.Add(restriction);
    }

    /// <summary>
    /// Clears the coupon restrictions.
    /// </summary>
    public void ClearRestrictions()
    {
        _restrictions.Clear();
    }

    /// <inheritdoc/>
    public void ToggleActivation()
    {
        IsActive = !IsActive;
    }
}
