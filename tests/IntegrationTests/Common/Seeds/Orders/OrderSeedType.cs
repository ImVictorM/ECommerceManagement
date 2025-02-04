namespace IntegrationTests.Common.Seeds.Orders;

/// <summary>
/// Represents the types of orders available in the database seed.
/// </summary>
public enum OrderSeedType
{
    /// <summary>
    /// Represents a pending order.
    /// </summary>
    CUSTOMER_ORDER_PENDING,
    /// <summary>
    /// Represents a canceled order.
    /// </summary>
    CUSTOMER_ORDER_CANCELED,
    /// <summary>
    /// Represents a paid order.
    /// </summary>
    CUSTOMER_ORDER_PAID,
}
