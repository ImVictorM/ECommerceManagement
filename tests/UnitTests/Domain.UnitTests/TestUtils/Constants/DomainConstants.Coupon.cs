namespace Domain.UnitTests.TestUtils.Constants;

public static partial class DomainConstants
{
    /// <summary>
    /// Defines constants for a coupon object.
    /// </summary>
    public static class Coupon
    {
        /// <summary>
        /// Defines the coupon discount percentage constant.
        /// </summary>
        public const int DiscountPercentage = 5;
        /// <summary>
        /// Defines the coupon code constant.
        /// </summary>
        public const string Code = "BLACK_FRIDAY";
        /// <summary>
        /// Defines the coupon usage limit constant.
        /// </summary>
        public const int UsageLimit = 10;
        /// <summary>
        /// Defines the coupon minimum price constant.
        /// </summary>
        public const decimal MinPrice = 0m;
        /// <summary>
        /// Defines the coupon auto apply constant.
        /// </summary>
        public const bool AutoApply = false;
    }
}
