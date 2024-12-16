using Application.Common.Interfaces.Persistence;
using Application.Orders.Commands.PlaceOrder;
using Application.UnitTests.Orders.Commands.TestUtils;
using Application.UnitTests.Orders.TestUtils.Extensions;
using Application.UnitTests.TestUtils.Behaviors;
using Domain.OrderAggregate;
using Domain.OrderAggregate.Services;
using Domain.OrderAggregate.ValueObjects;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Orders.Commands.PlaceOrder;

/// <summary>
/// Tests for the <see cref="PlaceOrderCommandHandler"/> command handler.
/// </summary>
public class PlaceOrderCommandHandlerTests
{
    private readonly PlaceOrderCommandHandler _handler;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IOrderProductServices> _mockOrdersServices;
    private readonly Mock<IRepository<Order, OrderId>> _mockOrderRepository;

    /// <summary>
    /// Initiates a new instance of the <see cref="PlaceOrderCommandHandlerTests"/> class.
    /// </summary>
    public PlaceOrderCommandHandlerTests()
    {
        _mockOrderRepository = new Mock<IRepository<Order, OrderId>>();
        _mockOrdersServices = new Mock<IOrderProductServices>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _mockUnitOfWork.Setup(uow => uow.OrderRepository).Returns(_mockOrderRepository.Object);

        _handler = new PlaceOrderCommandHandler(
            _mockUnitOfWork.Object,
            _mockOrdersServices.Object
        );
    }

    /// <summary>
    /// Tests the command handler handles a valid command successfully, creating an order and saving it.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task HandlePlaceOrder_WhenRequestIsValid_CreatesOrder()
    {
        var mockCreatedId = OrderId.Create(2);

        _mockOrdersServices
            .Setup(s => s.VerifyInventoryAvailabilityAsync(It.IsAny<IEnumerable<OrderProduct>>()))
            .Returns(Task.CompletedTask);

        _mockOrdersServices
            .Setup(s => s.CalculateTotalAsync(It.IsAny<IEnumerable<OrderProduct>>()))
            .ReturnsAsync(120m);

        MockEFCoreBehaviors.MockSetEntityIdBehavior(_mockOrderRepository, _mockUnitOfWork, mockCreatedId);

        var command = PlaceOrderCommandUtils.CreateCommand();

        var result = await _handler.Handle(command, default);

        result.Id.Should().Be(mockCreatedId.ToString());

        _mockOrdersServices.Verify(s => s.VerifyInventoryAvailabilityAsync(command.Products.ToOrderProduct()), Times.Once());
        _mockOrdersServices.Verify(s => s.CalculateTotalAsync(command.Products.ToOrderProduct()), Times.Once());

        _mockOrderRepository.Verify(r => r.AddAsync(It.IsAny<Order>()), Times.Once());
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once());
    }
}
