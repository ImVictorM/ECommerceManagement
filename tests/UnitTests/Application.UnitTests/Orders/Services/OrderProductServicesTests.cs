using System.Linq.Expressions;
using Application.Common.Interfaces.Persistence;
using Application.Orders.Commands.Common.Errors;
using Application.Orders.Services;
using Domain.OrderAggregate.ValueObjects;
using Domain.ProductAggregate;
using Domain.ProductAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Orders.Services;

/// <summary>
/// Tests the order product services implementations.
/// </summary>
public class OrderProductServicesTests
{
    private readonly OrderService _orderServices;
    private readonly Mock<IRepository<Product, ProductId>> _mockProductRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderProductServicesTests"/> class.
    /// </summary>
    public OrderProductServicesTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockProductRepository = new Mock<IRepository<Product, ProductId>>();
        _mockUnitOfWork.Setup(uow => uow.ProductRepository).Returns(_mockProductRepository.Object);
        _orderServices = new OrderService(_mockUnitOfWork.Object);
    }

    /// <summary>
    /// Tests the <see cref="OrderService.HasInventoryAvailableAsync(IEnumerable{OrderProduct})"/> method.
    /// Scenario: When some of the products does not have the quantity in inventory needed.
    /// </summary>
    [Fact]
    public async Task VerifyAvailability_WhenSomeProductDoesNotHaveQuantityNeededInInventory_ThrowsError()
    {
        var productWithoutStockId = ProductId.Create(2);

        IEnumerable<OrderProduct> mockInput = [OrderUtils.CreateOrderProduct(productId: productWithoutStockId, quantity: 10)];
        var mockProductUnavailable = ProductUtils.CreateProduct(id: productWithoutStockId, quantityAvailable: 3);

        _mockProductRepository
            .Setup(r => r.FindAllAsync(It.IsAny<Expression<Func<Product, bool>>>()))
            .ReturnsAsync([mockProductUnavailable]);

        await FluentActions
            .Invoking(() => _orderServices.HasInventoryAvailableAsync(mockInput))
            .Should()
            .ThrowAsync<ProductNotAvailableException>();
    }

    /// <summary>
    /// Tests the <see cref="OrderService.CalculateTotalAsync(IEnumerable{OrderProduct})"/> method.
    /// Tests if it calculates the total correctly.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task CalculateTotal_WhenCalculatingTheTotalOfTheOrder_ReturnsCorrectValue()
    {
        IEnumerable<OrderProduct> mockInputProducts = [
            OrderUtils.CreateOrderProduct(productId: ProductId.Create(1), quantity: 1),
            OrderUtils.CreateOrderProduct(productId: ProductId.Create(2), quantity: 2),
            OrderUtils.CreateOrderProduct(productId: ProductId.Create(3), quantity: 3)
        ];
        IEnumerable<Product> products = [
            ProductUtils.CreateProduct(id: ProductId.Create(1), price: 50),
            ProductUtils.CreateProduct(id: ProductId.Create(2), price: 120),
        ];

        var expectedTotal = 2990m;

        _mockProductRepository
            .Setup(r => r.FindAllAsync(It.IsAny<Expression<Func<Product, bool>>>()))
            .ReturnsAsync(products);

        var result = await _orderServices.CalculateTotalAsync(mockInputProducts);

        result.Should().Be(expectedTotal);
    }
}
