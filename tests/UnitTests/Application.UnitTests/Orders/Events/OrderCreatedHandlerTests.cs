using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Services;
using Application.Orders.Events;
using Application.UnitTests.Orders.Events.TestUtils;
using Domain.OrderAggregate;
using Domain.OrderAggregate.Enumerations;
using Domain.OrderAggregate.ValueObjects;
using Domain.PaymentAggregate;
using Domain.PaymentAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;
using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;
using FluentAssertions;
using Moq;
using SharedKernel.ValueObjects;

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
    /// List of not found users.
    /// </summary>
    public static IEnumerable<object[]> NotFoundUser()
    {
        yield return new object[] { (User?)null! };
        yield return new object[] { UserUtils.CreateUser(active: false) };
    }

    /// <summary>
    /// Tests the order created handler when notification is valid, verifying:
    /// - Addresses have been assigned to the user
    /// - Payment was created and saved
    /// - Payment authorization process was initiated.
    /// </summary>
    [Fact]
    public async Task HandleOrderCreated_WhenEventIsFiredAndInputIsValid_CreatesPaymentAndTriggersAuthorizationProcess()
    {
        var orderOwner = UserUtils.CreateUser();
        var orderCreatedEvent = OrderCreatedUtils.CreateEvent();

        _mockUserRepository.Setup(r => r.FindByIdAsync(It.IsAny<UserId>())).ReturnsAsync(orderOwner);

        await _eventHandler.Handle(orderCreatedEvent, default);

        orderOwner.UserAddresses.Should().Contain(orderCreatedEvent.BillingAddress);
        orderOwner.UserAddresses.Should().Contain(orderCreatedEvent.DeliveryAddress);

        _mockUserRepository.Verify(r => r.UpdateAsync(orderOwner), Times.Once());
        _mockPaymentRepository.Verify(r => r.AddAsync(It.IsAny<Payment>()), Times.Once());

        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once());

        _mockPaymentGateway.Verify(
           g => g.AuthorizePaymentAsync(
               It.IsAny<Payment>(),
               It.IsAny<User?>(),
               It.IsAny<Address?>(),
               It.IsAny<Address?>()
           ),
           Times.Once()
       );
    }

    /// <summary>
    /// Tests when the order owner is not found or inactive, the order is canceled.
    /// </summary>
    [Theory]
    [MemberData(nameof(NotFoundUser))]
    public async Task HandleOrderCreated_WhenUserCouldNotBeFound_CancelsOrder(User? userNotFound)
    {
        _mockUserRepository.Setup(r => r.FindByIdAsync(It.IsAny<UserId>())).ReturnsAsync(userNotFound);

        var orderCreatedEvent = OrderCreatedUtils.CreateEvent();

        await _eventHandler.Handle(orderCreatedEvent, default);

        orderCreatedEvent.Order.OrderStatusId.Should().Be(OrderStatus.Canceled.Id);

        _mockPaymentRepository.Verify(r => r.AddAsync(It.IsAny<Payment>()), Times.Never());
        _mockPaymentGateway.Verify(
            g => g.AuthorizePaymentAsync(
                It.IsAny<Payment>(),
                It.IsAny<User?>(),
                It.IsAny<Address?>(),
                It.IsAny<Address?>()
            ),
            Times.Never()
        );

        _mockOrderRepository.Verify(r => r.UpdateAsync(orderCreatedEvent.Order), Times.Once());
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once());
    }
}
