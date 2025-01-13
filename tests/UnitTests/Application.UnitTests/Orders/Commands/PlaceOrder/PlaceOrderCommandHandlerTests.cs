using Application.Common.Interfaces.Persistence;
using Application.Orders.Commands.PlaceOrder;
using Application.UnitTests.Orders.Commands.TestUtils;
using Application.UnitTests.Orders.TestUtils.Extensions;
using Application.UnitTests.TestUtils.Behaviors;

using Domain.OrderAggregate;
using Domain.OrderAggregate.Services;
using Domain.OrderAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;

using FluentAssertions;
using Moq;

namespace Application.UnitTests.Orders.Commands.PlaceOrder;

/// <summary>
/// Unit tests for the <see cref="PlaceOrderCommandHandler"/> command handler.
/// </summary>
public class PlaceOrderCommandHandlerTests
{
    private readonly PlaceOrderCommandHandler _handler;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IOrderService> _mockOrdersService;
    private readonly Mock<IRepository<Order, OrderId>> _mockOrderRepository;

    /// <summary>
    /// Initiates a new instance of the <see cref="PlaceOrderCommandHandlerTests"/> class.
    /// </summary>
    public PlaceOrderCommandHandlerTests()
    {
        _mockOrderRepository = new Mock<IRepository<Order, OrderId>>();
        _mockOrdersService = new Mock<IOrderService>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _mockUnitOfWork.Setup(uow => uow.OrderRepository).Returns(_mockOrderRepository.Object);

        _handler = new PlaceOrderCommandHandler(
            _mockUnitOfWork.Object,
            _mockOrdersService.Object
        );
    }

    /// <summary>
    /// Tests the command handler handles a valid command successfully, creating an order and saving it.
    /// </summary>
    [Fact]
    public async Task HandlePlaceOrder_WhenRequestIsValid_CreatesOrder()
    {
        var reservedProducts = OrderUtils.CreateReservedProducts(3);
        var orderProducts = reservedProducts
            .Select(rp => OrderUtils.CreateOrderProduct(productId: rp.ProductId, quantity: rp.Quantity));
        var mockTotal = orderProducts.Sum(op => op.CalculateTransactionPrice());

        var command = PlaceOrderCommandUtils.CreateCommand(
            orderProducts: reservedProducts.ParseToInput(),
            couponsAppliedIds: ["1", "2"],
            installments: 2
        );

        _mockOrdersService
            .Setup(s => s.PrepareOrderProductsAsync(command.Products))
            .Returns(orderProducts.ToAsyncEnumerable());

        _mockOrdersService
            .Setup(s => s.CalculateTotalAsync(orderProducts, It.IsAny<IEnumerable<OrderCoupon>>()))
            .ReturnsAsync(mockTotal);

        var mockCreatedId = OrderId.Create(2);
        MockEFCoreBehaviors.MockSetEntityIdBehavior(_mockOrderRepository, _mockUnitOfWork, mockCreatedId);

        var result = await _handler.Handle(command, default);

        result.Id.Should().Be(mockCreatedId.ToString());

        _mockOrderRepository.Verify(
            r => r.AddAsync(It.Is<Order>(o => o.Total == mockTotal && o.Products == orderProducts)),
            Times.Once()
        );
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once());
    }
}
