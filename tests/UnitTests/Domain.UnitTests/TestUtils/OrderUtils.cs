using Domain.CategoryAggregate.ValueObjects;
using Domain.OrderAggregate;
using Domain.OrderAggregate.Factories;
using Domain.OrderAggregate.Interfaces;
using Domain.OrderAggregate.Services;
using Domain.OrderAggregate.ValueObjects;
using Domain.ProductAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;
using Domain.OrderAggregate.Enumerations;

using SharedKernel.Interfaces;
using SharedKernel.UnitTests.TestUtils;
using SharedKernel.UnitTests.TestUtils.Extensions;
using SharedKernel.ValueObjects;

using Moq;
using Bogus;

namespace Domain.UnitTests.TestUtils;

/// <summary>
/// Utilities for the <see cref="Order"/> class.
/// </summary>
public static class OrderUtils
{
    private static readonly Faker _faker = new();

    /// <summary>
    /// Creates a new fake instance of the <see cref="Order"/> class.
    /// </summary>
    /// <param name="requestId">The request identifier.</param>
    /// <param name="id">The order id.</param>
    /// <param name="ownerId">The order owner id.</param>
    /// <param name="orderProducts">The order products to be processed.</param>
    /// <param name="paymentMethod">The order payment method.</param>
    /// <param name="billingAddress">The order billing address.</param>
    /// <param name="deliveryAddress">The order delivery address.</param>
    /// <param name="installments">The order installments.</param>
    /// <param name="couponsApplied">The order coupons applied.</param>
    /// <param name="orderService">The order service.</param>
    /// <param name="initialOrderStatus">The initial order status.</param>
    /// <returns>A new fake instance of the <see cref="Order"/> class.</returns>
    public static async Task<Order> CreateOrderAsync(
        Guid? requestId = null,
        OrderId? id = null,
        UserId? ownerId = null,
        IEnumerable<IOrderProductReserved>? orderProducts = null,
        IPaymentMethod? paymentMethod = null,
        Address? billingAddress = null,
        Address? deliveryAddress = null,
        int? installments = null,
        IEnumerable<OrderCoupon>? couponsApplied = null,
        IOrderService? orderService = null,
        OrderStatus? initialOrderStatus = null
    )
    {
        var products = orderProducts ?? CreateReservedProducts();
        var mockOrderService = orderService ?? CreateMockOrderService(products);

        var factory = new OrderFactory(mockOrderService);

        var order = await factory.CreateOrderAsync(
            requestId ?? _faker.Random.Guid(),
            ownerId ?? UserId.Create(_faker.Random.Long()),
            products,
            paymentMethod ?? CreateMockPaymentMethod(),
            billingAddress ?? AddressUtils.CreateAddress(),
            deliveryAddress ?? AddressUtils.CreateAddress(),
            installments,
            couponsApplied
        );

        if (id != null)
        {
            order.SetIdUsingReflection(id);
        }

        if (initialOrderStatus is not null)
        {
            var statusIdProperty = typeof(Order).GetProperty(
                nameof(Order.OrderStatusId),
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance
            );

            if (statusIdProperty != null && statusIdProperty.CanWrite)
            {
                statusIdProperty.SetValue(order, initialOrderStatus.Id);
            }
        }

        return order;
    }

    /// <summary>
    /// Generates a single instance of <see cref="IOrderProductReserved"/>.
    /// </summary>
    /// <param name="productId">The product id.</param>
    /// <param name="quantity">The product quantity.</param>
    /// <returns>A new instance of <see cref="IOrderProductReserved"/>.</returns>
    public static IOrderProductReserved CreateReservedProduct(
        ProductId? productId = null,
        int? quantity = null
    )
    {
        return new Faker<IOrderProductReserved>()
            .CustomInstantiator(_ => CreateOrderProductReservedMock(productId, quantity))
            .Generate();
    }

    /// <summary>
    /// Generates fake instances of <see cref="IOrderProductReserved"/>.
    /// </summary>
    /// <returns>A list of reserved products.</returns>
    public static IReadOnlyCollection<IOrderProductReserved> CreateReservedProducts(int count = 1)
    {
        return new Faker<IOrderProductReserved>()
            .CustomInstantiator(_ => CreateOrderProductReservedMock())
            .Generate(count);
    }

    /// <summary>
    /// Creates a new fake instance of <see cref="OrderProduct"/>.
    /// </summary>
    /// <returns>A new fake instance of <see cref="OrderProduct"/>.</returns>
    public static OrderProduct CreateOrderProduct(
        ProductId? productId = null,
        int? quantity = null,
        decimal? basePrice = null,
        decimal? purchasedPrice = null,
        IReadOnlySet<CategoryId>? productCategories = null
    )
    {
        return OrderProduct.Create(
            productId ?? ProductId.Create(_faker.Random.Int(1, 1000)),
            quantity ?? _faker.Random.Int(1, 10),
            basePrice ?? _faker.Finance.Amount(10, 100),
            purchasedPrice ?? _faker.Finance.Amount(10, 100),
            productCategories ?? new HashSet<CategoryId>
            {
                CategoryId.Create(_faker.Random.Int(1, 10))
            }
        );
    }

    /// <summary>
    /// Creates a new fake instance of <see cref="IPaymentMethod"/>.
    /// </summary>
    /// <returns>A new fake instance of <see cref="IPaymentMethod"/>.</returns>
    public static IPaymentMethod CreateMockPaymentMethod()
    {
        var mock = new Mock<IPaymentMethod>();

        mock.SetupGet(pm => pm.Type).Returns("mock_payment");

        return mock.Object;
    }

    private static IOrderService CreateMockOrderService(IEnumerable<IOrderProductReserved> orderProducts)
    {
        var mock = new Mock<IOrderService>();

        var preparedProducts = orderProducts.Select(op => CreateOrderProduct(op.ProductId, op.Quantity));
        var preparedProductsTotal = preparedProducts.Sum(pp => pp.CalculateTransactionPrice());

        mock.Setup(s => s.PrepareOrderProductsAsync(orderProducts))
            .Returns(preparedProducts.ToAsyncEnumerable());

        mock.Setup(s => s.CalculateTotalAsync(It.IsAny<IEnumerable<OrderProduct>>(), It.IsAny<IEnumerable<OrderCoupon>>()))
            .ReturnsAsync(preparedProductsTotal);

        return mock.Object;
    }

    private static IOrderProductReserved CreateOrderProductReservedMock(
         ProductId? productId = null,
        int? quantity = null
    )
    {
        var id = productId ?? ProductId.Create(_faker.Random.Int(1, 100));
        var productQuantity = quantity ?? _faker.Random.Int(1, 10);

        var mock = new Mock<IOrderProductReserved>();
        mock.SetupGet(op => op.ProductId).Returns(id);
        mock.SetupGet(op => op.Quantity).Returns(productQuantity);

        return mock.Object;
    }
}
