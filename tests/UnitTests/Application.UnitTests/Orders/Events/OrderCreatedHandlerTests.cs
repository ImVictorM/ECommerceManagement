using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Services;
using Application.Orders.Events;
using Application.UnitTests.Orders.Events.TestUtils;

using Domain.OrderAggregate;
using Domain.OrderAggregate.ValueObjects;
using Domain.PaymentAggregate;
using Domain.PaymentAggregate.ValueObjects;
using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;

using Moq;

namespace Application.UnitTests.Orders.Events;

/// <summary>
/// Tests for the <see cref="OrderCreatedHandler"/> event handler.
/// </summary>
public class OrderCreatedHandlerTests
{
    private readonly OrderCreatedHandler _eventHandler;
    private readonly Mock<IPaymentGateway> _mockPaymentGateway;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRepository<Payment, PaymentId>> _mockPaymentRepository;
    private readonly Mock<IRepository<User, UserId>> _mockUserRepository;
    private readonly Mock<IRepository<Order, OrderId>> _mockOrderRepository;

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderCreatedHandlerTests"/> class.
    /// </summary>
    public OrderCreatedHandlerTests()
    {
        _mockPaymentGateway = new Mock<IPaymentGateway>();
        _mockPaymentRepository = new Mock<IRepository<Payment, PaymentId>>();
        _mockUserRepository = new Mock<IRepository<User, UserId>>();
        _mockOrderRepository = new Mock<IRepository<Order, OrderId>>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _mockUnitOfWork.Setup(uow => uow.PaymentRepository).Returns(_mockPaymentRepository.Object);
        _mockUnitOfWork.Setup(uow => uow.UserRepository).Returns(_mockUserRepository.Object);
        _mockUnitOfWork.Setup(uow => uow.OrderRepository).Returns(_mockOrderRepository.Object);

        _eventHandler = new OrderCreatedHandler(_mockPaymentGateway.Object, _mockUnitOfWork.Object);
    }

    /// <summary>
    /// Tests a payment is created when the event is fired.
    /// </summary>
    [Fact]
    public async Task HandleOrderCreated_WhenEventIsFired_CreatesPayment()
    {
        var orderCreatedEvent = await OrderCreatedUtils.CreateEvent();

        await _eventHandler.Handle(orderCreatedEvent, default);

        _mockPaymentRepository.Verify(r => r.AddAsync(It.IsAny<Payment>()), Times.Once());

        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once());
    }
}
