using Application.Products.Events;
using Application.UnitTests.TestUtils.Events.Orders;
using Application.Common.Persistence;

using Domain.ProductAggregate;
using Domain.ProductAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;

using System.Linq.Expressions;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Products.Events;

/// <summary>
/// Unit tests for the <see cref="OrderCanceledRestockProductsHandler"/> event handler.
/// </summary>
public class OrderCanceledRestockProductsHandlerTests
{
    private readonly OrderCanceledRestockProductsHandler _handler;
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderCanceledRestockProductsHandlerTests"/> class.
    /// </summary>
    public OrderCanceledRestockProductsHandlerTests()
    {
        _mockProductRepository = new Mock<IProductRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _handler = new OrderCanceledRestockProductsHandler(
            _mockUnitOfWork.Object,
            _mockProductRepository.Object
        );
    }

    /// <summary>
    /// Verifies the handler restock the products of the canceled order.
    /// </summary>
    [Fact]
    public async Task HandleOrderCanceled_WithCanceledOrder_RestockTheOrderProducts()
    {
        var products = new[]
        {
            ProductUtils.CreateProduct(id: ProductId.Create(1), initialQuantityInInventory: 10),
            ProductUtils.CreateProduct(id: ProductId.Create(2), initialQuantityInInventory: 20),
            ProductUtils.CreateProduct(id: ProductId.Create(3), initialQuantityInInventory: 30),
        };

        var reservedProducts = new[]
        {
            OrderUtils.CreateReservedProduct(productId: products[0].Id, 1),
            OrderUtils.CreateReservedProduct(productId: products[1].Id, 2),
            OrderUtils.CreateReservedProduct(productId: products[2].Id, 3)
        };

        var order = await OrderUtils.CreateOrderAsync(orderProducts: reservedProducts);

        var notification = await OrderCanceledUtils.CreateEventAsync(order: order);

        _mockProductRepository
            .Setup(r => r.FindAllAsync(
                It.IsAny<Expression<Func<Product, bool>>>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(products);

        await _handler.Handle(notification, default);

        products[0].Inventory.QuantityAvailable.Should().Be(11);
        products[1].Inventory.QuantityAvailable.Should().Be(22);
        products[2].Inventory.QuantityAvailable.Should().Be(33);
    }
}
