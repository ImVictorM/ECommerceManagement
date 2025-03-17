namespace IntegrationTests.Common.Seeds.Coupons;

/// <summary>
/// Represents the types of coupons available in the database seed.
/// </summary>
public enum CouponSeedType
{
    /// <summary>
    /// Represents a technology coupon.
    /// </summary>
    TECH_COUPON,

    /// <summary>
    /// Represents a 10% discount coupon that is not usable anymore.
    /// </summary>
    PAST10,

    /// <summary>
    /// Represents a 15% summer sale coupon.
    /// </summary>
    SUMMER15,

    /// <summary>
    /// Represents a 20% welcome coupon that is inactive.
    /// </summary>
    WELCOME20_INACTIVE,

    /// <summary>
    /// Represents a 50% Black Friday coupon.
    /// </summary>
    BLACKFRIDAY50
}
