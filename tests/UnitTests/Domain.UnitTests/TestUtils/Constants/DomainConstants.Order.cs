using Domain.OrderAggregate.ValueObjects;
using Domain.ProductAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;

namespace Domain.UnitTests.TestUtils.Constants;

public static partial class DomainConstants
{
    /// <summary>
    /// Defines constants for an order.
    /// </summary>
    public static class Order
    {
        /// <summary>
        /// The order constant id.
        /// </summary>
        public static readonly OrderId Id = OrderId.Create(1);

        /// <summary>
        /// The order constant owner id.
        /// </summary>
        public static readonly UserId OwnerId = UserId.Create(1);

        /// <summary>
        /// The order constant description.
        /// </summary>
        public const string Description = "test description";

        /// <summary>
        /// The order constant total.
        /// </summary>
        public const decimal Total = 120m;

        /// <summary>
        /// The order constant products.
        /// </summary>
        public static readonly IEnumerable<OrderProduct> OrderProducts =
        [
            OrderProduct.Create(ProductId.Create(1), 1),
            OrderProduct.Create(ProductId.Create(2), 3),
            OrderProduct.Create(ProductId.Create(3), 1),
        ];
    }
}
