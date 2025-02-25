using Application.Common.Persistence.Repositories;
using Application.Orders.Services;
using Application.Orders.Errors;

using Domain.CouponAggregate;
using Domain.CouponAggregate.ValueObjects;
using Domain.OrderAggregate.ValueObjects;
using Domain.ProductAggregate;
using Domain.ProductAggregate.Services;
using Domain.ProductAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;
using Domain.ShippingMethodAggregate.ValueObjects;
using Domain.ShippingMethodAggregate;

using SharedKernel.UnitTests.TestUtils;
using SharedKernel.Interfaces;
using SharedKernel.ValueObjects;

using System.Linq.Expressions;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Orders.Services;

/// <summary>
/// Unit tests for the <see cref="OrderService"/> service implementation.
/// </summary>
public class OrderServiceTests
{
    private readonly OrderService _service;
    private readonly Mock<IProductService> _mockProductService;
    private readonly Mock<IShippingMethodRepository> _mockShippingMethodRepository;
    private readonly Mock<ICouponRepository> _mockCouponRepository;
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly Mock<IDiscountService> _mockDiscountService;

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderServiceTests"/> class.
    /// </summary>
    public OrderServiceTests()
    {
        _mockProductService = new Mock<IProductService>();
        _mockShippingMethodRepository = new Mock<IShippingMethodRepository>();
        _mockCouponRepository = new Mock<ICouponRepository>();
        _mockProductRepository = new Mock<IProductRepository>();
        _mockDiscountService = new Mock<IDiscountService>();

        _service = new OrderService(
            _mockProductService.Object,
            _mockShippingMethodRepository.Object,
            _mockCouponRepository.Object,
            _mockProductRepository.Object,
            _mockDiscountService.Object
        );
    }

    /// <summary>
    /// Tests the calculation of the total price when no coupon is applied.
    /// </summary>
    [Fact]
    public async Task CalculateTotal_WithoutCouponsApplied_ReturnsProductsTotal()
    {
        IEnumerable<OrderProduct> orderProducts = [
            OrderUtils.CreateOrderProduct(
                purchasedPrice: 5m,
                quantity: 1
            ),
            OrderUtils.CreateOrderProduct(
                purchasedPrice: 10m,
                quantity: 2
            )
        ];

        var shippingMethod = ShippingMethodUtils.CreateShippingMethod(id: ShippingMethodId.Create(1));

        var productsTotal = orderProducts.Sum(p => p.CalculateTransactionPrice());
        var expectedTotal = productsTotal + shippingMethod.Price;

        _mockShippingMethodRepository
            .Setup(r => r.FindByIdAsync(
                shippingMethod.Id,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(shippingMethod);

        _mockDiscountService
            .Setup(s => s.CalculateDiscountedPrice(
                productsTotal,
                It.IsAny<IEnumerable<Discount>>()
            ))
            .Returns(expectedTotal);

        var result = await _service.CalculateTotalAsync(orderProducts, shippingMethod.Id, null);

        result.Should().Be(expectedTotal);
    }

    /// <summary>
    /// Tests the calculation of the total price when valid coupons are applied.
    /// </summary>
    [Fact]
    public async Task CalculateTotal_WithValidCouponsApplied_ReturnsProductsTotalWithDiscount()
    {
        var orderProducts = new[]
        {
            OrderUtils.CreateOrderProduct(purchasedPrice: 50m, quantity: 1),
            OrderUtils.CreateOrderProduct(purchasedPrice: 30m, quantity: 2)
        };

        var orderCoupons = new[]
        {
            OrderCoupon.Create(CouponId.Create(1)),
            OrderCoupon.Create(CouponId.Create(2))
        };

        var couponsApplied = new[]
        {
            CouponUtils.CreateCoupon(
                CouponId.Create(1),
                discount: DiscountUtils.CreateDiscount(
                    PercentageUtils.Create(2),
                    startingDate: DateTimeOffset.UtcNow.AddHours(-5),
                    endingDate: DateTimeOffset.UtcNow.AddHours(5)
                ),
                usageLimit: 100,
                minPrice: 0m
            ),
            CouponUtils.CreateCoupon(
                CouponId.Create(2),
                discount: DiscountUtils.CreateDiscount(
                    PercentageUtils.Create(5),
                    startingDate: DateTimeOffset.UtcNow.AddHours(-5),
                    endingDate: DateTimeOffset.UtcNow.AddHours(5)
                ),
                usageLimit: 100,
                minPrice: 0m
            )
        };

        var shippingMethod = ShippingMethodUtils.CreateShippingMethod(
            id: ShippingMethodId.Create(1),
            price: 10m
        );

        var productsTotal = orderProducts.Sum(p => p.PurchasedPrice * p.Quantity);

        var discounts = couponsApplied
            .Select(c => c.Discount);

        var totalApplyingDiscountsInDescendingOrder = 102.41m;

        var expectedTotal = totalApplyingDiscountsInDescendingOrder + shippingMethod.Price;

        _mockShippingMethodRepository
            .Setup(r => r.FindByIdAsync(
                shippingMethod.Id,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(shippingMethod);

        _mockCouponRepository
            .Setup(r => r.FindAllAsync(
                It.IsAny<Expression<Func<Coupon, bool>>>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(couponsApplied);

        _mockDiscountService
            .Setup(s => s.CalculateDiscountedPrice(productsTotal, discounts))
            .Returns(totalApplyingDiscountsInDescendingOrder);

        var result = await _service.CalculateTotalAsync(orderProducts, shippingMethod.Id, orderCoupons);

        result.Should().Be(expectedTotal);
    }

    /// <summary>
    /// Tests the calculation of the total price when invalid coupons are applied.
    /// </summary>
    [Fact]
    public async Task CalculateTotal_WithInvalidCoupons_ThrowsError()
    {
        var orderProducts = new[]
        {
            OrderUtils.CreateOrderProduct(purchasedPrice: 50m, quantity: 1),
            OrderUtils.CreateOrderProduct(purchasedPrice: 30m, quantity: 2)
        };

        var orderCoupons = new[]
        {
            OrderCoupon.Create(CouponId.Create(1)),
        };

        var invalidCoupons = new[]
        {
            CouponUtils.CreateCoupon(
                id: CouponId.Create(1),
                active: false
            ),
        };

        var shippingMethod = ShippingMethodUtils.CreateShippingMethod();

        _mockShippingMethodRepository
            .Setup(r => r.FindByIdAsync(
                shippingMethod.Id,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(shippingMethod);

        _mockCouponRepository
            .Setup(r => r.FindAllAsync(
                It.IsAny<Expression<Func<Coupon, bool>>>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(invalidCoupons);

        await FluentActions
            .Invoking(() => _service.CalculateTotalAsync(orderProducts, shippingMethod.Id, orderCoupons))
            .Should()
            .ThrowAsync<InvalidCouponAppliedException>();
    }

    /// <summary>
    /// Tests the calculation of the total price when invalid coupons are applied.
    /// </summary>
    [Fact]
    public async Task CalculateTotal_WithInvalidShippingMethod_ThrowsError()
    {
        var orderProducts = new[]
        {
            OrderUtils.CreateOrderProduct(purchasedPrice: 50m, quantity: 1),
            OrderUtils.CreateOrderProduct(purchasedPrice: 30m, quantity: 2)
        };

        var orderCoupons = new[]
        {
            OrderCoupon.Create(CouponId.Create(1)),
        };

        var couponsApplied = new[]
        {
            CouponUtils.CreateCoupon(
                id: CouponId.Create(1),
                active: false
            ),
        };

        _mockShippingMethodRepository
            .Setup(r => r.FindByIdAsync(
                It.IsAny<ShippingMethodId>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync((ShippingMethod?)null);

        _mockCouponRepository
            .Setup(r => r.FindAllAsync(
                It.IsAny<Expression<Func<Coupon, bool>>>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(couponsApplied);

        await FluentActions
            .Invoking(() => _service.CalculateTotalAsync(orderProducts, ShippingMethodId.Create(1), orderCoupons))
            .Should()
            .ThrowAsync<InvalidShippingMethodException>();
    }

    /// <summary>
    /// Tests the preparation of order products when there is sufficient inventory
    /// considering the products are not on sale.
    /// </summary>
    [Fact]
    public async Task PrepareOrderProducts_WithSufficientInventory_ReturnsPreparedProducts()
    {
        var orderProductsInput = new[]
        {
            OrderUtils.CreateOrderProduct(productId: ProductId.Create(1), quantity: 2),
            OrderUtils.CreateOrderProduct(productId: ProductId.Create(2), quantity: 1)
        };

        var products = new[]
        {
            ProductUtils.CreateProduct(id: ProductId.Create(1), basePrice: 100m, initialQuantityInInventory: 10),
            ProductUtils.CreateProduct(id: ProductId.Create(2), basePrice: 50m, initialQuantityInInventory: 5)
        };

        var productsWithPrices = products.ToDictionary(p => p.Id, p => p.BasePrice);

        var expectedOrderProductsPrepared = orderProductsInput.Zip(
            products,
            (first, second) => OrderProduct.Create(
                first.ProductId,
                first.Quantity,
                second.BasePrice,
                second.BasePrice,
                second.ProductCategories.Select(pc => pc.CategoryId).ToHashSet()
            )
        ).ToList();

        _mockProductRepository
            .Setup(r => r.FindAllAsync(
                It.IsAny<Expression<Func<Product, bool>>>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(products);

        _mockProductService
            .Setup(ps => ps.CalculateProductsPriceApplyingSaleAsync(
                products,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(productsWithPrices);

        var result = await _service.PrepareOrderProductsAsync(orderProductsInput);

        result.Should().HaveCount(2);

        result.Should().BeEquivalentTo(expectedOrderProductsPrepared);
    }

    /// <summary>
    /// Tests the preparation of order products when there is insufficient inventory.
    /// </summary>
    [Fact]
    public async Task PrepareOrderProducts_WithInsufficientInventory_ThrowsError()
    {
        var orderProductsInput = new[]
        {
            OrderUtils.CreateOrderProduct(productId: ProductId.Create(1), quantity: 10),
        };

        var products = new[]
        {
            ProductUtils.CreateProduct(id: ProductId.Create(1), basePrice: 100m, initialQuantityInInventory: 1),
        };

        var productsWithPrices = products.ToDictionary(p => p.Id, p => p.BasePrice);

        _mockProductRepository
            .Setup(r => r.FindAllAsync(
                It.IsAny<Expression<Func<Product, bool>>>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(products);

        _mockProductService
            .Setup(ps => ps.CalculateProductsPriceApplyingSaleAsync(
                products,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(productsWithPrices);

        await FluentActions
            .Invoking(async () => await _service.PrepareOrderProductsAsync(orderProductsInput))
            .Should()
            .ThrowAsync<OrderProductNotAvailableException>();
    }
}
