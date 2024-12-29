using Application.Common.Interfaces.Persistence;
using Application.Orders.Common.Errors;
using Application.Orders.Services;

using Domain.CouponAggregate;
using Domain.CouponAggregate.ValueObjects;
using Domain.OrderAggregate.ValueObjects;
using Domain.ProductAggregate;
using Domain.ProductAggregate.Services;
using Domain.ProductAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;

using SharedKernel.UnitTests.TestUtils;

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
    private readonly Mock<IRepository<Product, ProductId>> _mockProductRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderServiceTests"/> class.
    /// </summary>
    public OrderServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockProductRepository = new Mock<IRepository<Product, ProductId>>();
        _mockProductService = new Mock<IProductService>();

        _mockUnitOfWork.Setup(uow => uow.ProductRepository).Returns(_mockProductRepository.Object);

        _service = new OrderService(_mockUnitOfWork.Object, _mockProductService.Object);
    }

    /// <summary>
    /// Tests the calculation of the total price when no coupon is applied.
    /// </summary>
    [Fact]
    public async Task CalculateTotal_WithoutCouponApplied_ReturnsProductsTotal()
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

        var expectedTotal = 25m;

        var result = await _service.CalculateTotalAsync(orderProducts, null);

        result.Should().Be(expectedTotal);
    }

    /// <summary>
    /// Tests the calculation of the total price when valid coupons are applied.
    /// </summary>
    [Fact]
    public async Task CalculateTotal_WithValidCoupons_AppliesDiscounts()
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
                discount: DiscountUtils.CreateDiscount(PercentageUtils.Create(2))
            ),
            CouponUtils.CreateCoupon(
                CouponId.Create(2),
                discount: DiscountUtils.CreateDiscount(PercentageUtils.Create(5))
            )
        };

        _mockUnitOfWork
            .Setup(uow => uow.CouponRepository.FindAllAsync(It.IsAny<Expression<Func<Coupon, bool>>>()))
            .ReturnsAsync(couponsApplied);

        var result = await _service.CalculateTotalAsync(orderProducts, orderCoupons);

        result.Should().Be(102.41m);
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

        var couponsApplied = new[]
        {
            CouponUtils.CreateCoupon(
                id: CouponId.Create(1),
                active: false
            ),
        };

        _mockUnitOfWork
            .Setup(uow => uow.CouponRepository.FindAllAsync(It.IsAny<Expression<Func<Coupon, bool>>>()))
            .ReturnsAsync(couponsApplied);

        await FluentActions
            .Invoking(() => _service.CalculateTotalAsync(orderProducts, orderCoupons))
            .Should()
            .ThrowAsync<InvalidOrderCouponAppliedException>();
    }

    /// <summary>
    /// Tests the preparation of order products when there is sufficient inventory.
    /// Considers the products are not on sale.
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

        _mockUnitOfWork
            .Setup(uow => uow.ProductRepository.FindAllAsync(It.IsAny<Expression<Func<Product, bool>>>()))
            .ReturnsAsync(products);

        _mockProductService
            .Setup(ps => ps.CalculateProductPriceApplyingSaleAsync(It.IsAny<Product>()))
            .ReturnsAsync((Product p) => p.BasePrice);

        var result = await _service.PrepareOrderProductsAsync(orderProductsInput).ToListAsync();

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

        _mockUnitOfWork
            .Setup(uow => uow.ProductRepository.FindAllAsync(It.IsAny<Expression<Func<Product, bool>>>()))
            .ReturnsAsync(products);

        _mockProductService
            .Setup(ps => ps.CalculateProductPriceApplyingSaleAsync(It.IsAny<Product>()))
            .ReturnsAsync((Product p) => p.BasePrice);

        await FluentActions
            .Invoking(async () => await _service.PrepareOrderProductsAsync(orderProductsInput).ToListAsync())
            .Should()
            .ThrowAsync<InventoryInsufficientException>();
    }
}
