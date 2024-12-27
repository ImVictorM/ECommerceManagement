using Domain.CategoryAggregate.ValueObjects;
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
        /// The order constant products.
        /// </summary>
        public static readonly IEnumerable<OrderProduct> OrderProducts =
        [
            OrderProduct.Create(
                productId: ProductId.Create(1),
                quantity: 1,
                basePrice: 15m,
                purchasedPrice: 15m,
                productCategories: new HashSet<CategoryId>()
                {
                    CategoryId.Create(1),
                    CategoryId.Create(2),
                }
            ),
           OrderProduct.Create(
                productId: ProductId.Create(2),
                quantity: 5,
                basePrice: 5m,
                purchasedPrice: 2m,
                productCategories: new HashSet<CategoryId>()
                {
                    CategoryId.Create(2),
                }
           ),
           OrderProduct.Create(
                productId: ProductId.Create(3),
                quantity: 2,
                basePrice: 10m,
                purchasedPrice: 9.5m,
                productCategories: new HashSet<CategoryId>()
                {
                    CategoryId.Create(5),
                }
           ),
        ];

        /// <summary>
        /// The order total constant.
        /// </summary>
        public const decimal Total = 26.5m;
    }
}
