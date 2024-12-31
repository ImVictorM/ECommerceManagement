using Application.Common.Errors;
using Application.Common.Interfaces.Payments;
using Application.Common.Interfaces.Persistence;
using Application.Orders.Events;
using Application.UnitTests.Orders.Events.TestUtils;

using Domain.OrderAggregate;
using Domain.OrderAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;
using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;

using SharedKernel.Interfaces;
using SharedKernel.ValueObjects;

using FluentAssertions;
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
    private readonly Mock<IRepository<User, UserId>> _mockUserRepository;
    private readonly Mock<IRepository<Order, OrderId>> _mockOrderRepository;

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderCreatedHandlerTests"/> class.
    /// </summary>
    public OrderCreatedHandlerTests()
    {
        _mockPaymentGateway = new Mock<IPaymentGateway>();
        _mockUserRepository = new Mock<IRepository<User, UserId>>();
        _mockOrderRepository = new Mock<IRepository<Order, OrderId>>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _mockUnitOfWork.Setup(uow => uow.UserRepository).Returns(_mockUserRepository.Object);
        _mockUnitOfWork.Setup(uow => uow.OrderRepository).Returns(_mockOrderRepository.Object);

        _eventHandler = new OrderCreatedHandler(_mockPaymentGateway.Object, _mockUnitOfWork.Object);
    }

    /// <summary>
    /// Tests a payment is created when the event is fired.
    /// </summary>
    [Fact]
    public async Task HandleOrderCreated_WhenEventIsFired_InitializePaymentAuthorization()
    {
        var payerId = UserId.Create(1);
        var payer = UserUtils.CreateUser(id: payerId);
        var order = await OrderUtils.CreateOrderAsync(ownerId: payerId);

        var orderCreatedEvent = await OrderCreatedUtils.CreateEvent(order: order);

        _mockUserRepository
            .Setup(repo => repo.FindFirstSatisfyingAsync(It.IsAny<ISpecificationQuery<User>>()))
            .ReturnsAsync(payer);

        await _eventHandler.Handle(orderCreatedEvent, default);

        _mockPaymentGateway.Verify(
            g => g.AuthorizePaymentAsync(
                It.Is<Order>(o => o == order),
                It.Is<IPaymentMethod>(pm => pm.Type == orderCreatedEvent.PaymentMethod.Type),
                It.Is<User>(u => u == payer),
                It.Is<Address>(address => address == orderCreatedEvent.BillingAddress),
                It.Is<Address>(address => address == orderCreatedEvent.DeliveryAddress),
                It.Is<int?>(i => i == orderCreatedEvent.Installments)
            ),
            Times.Once()
        );
    }

    /// <summary>
    /// Tests that a <see cref="UserNotFoundException"/> is thrown when the payer does not exist.
    /// </summary>
    [Fact]
    public async Task HandleOrderCreated_WhenPayerNotFound_ThrowsUserNotFoundException()
    {
        var orderCreatedEvent = await OrderCreatedUtils.CreateEvent();

        _mockUserRepository
            .Setup(repo => repo.FindFirstSatisfyingAsync(It.IsAny<ISpecificationQuery<User>>()))
            .ReturnsAsync((User?)null);

        await FluentActions
            .Invoking(() => _eventHandler.Handle(orderCreatedEvent, default))
            .Should()
            .ThrowAsync<UserNotFoundException>();

        _mockPaymentGateway.Verify(
            g => g.AuthorizePaymentAsync(
                It.IsAny<Order>(),
                It.IsAny<IPaymentMethod>(),
                It.IsAny<User>(),
                It.IsAny<Address>(),
                It.IsAny<Address>(),
                It.IsAny<int>()
            ),
            Times.Never()
        );
    }

    /// <summary>
    /// Tests that payer addresses are updated when the event is handled.
    /// </summary>
    [Fact]
    public async Task HandleOrderCreated_WhenEventIsFired_UpdatesPayerAddresses()
    {
        var payerId = UserId.Create(1);
        var payer = UserUtils.CreateUser(id: payerId);
        var order = await OrderUtils.CreateOrderAsync(ownerId: payerId);

        var orderCreatedEvent = await OrderCreatedUtils.CreateEvent(order: order);

        _mockUserRepository
            .Setup(repo => repo.FindFirstSatisfyingAsync(It.IsAny<ISpecificationQuery<User>>()))
            .ReturnsAsync(payer);

        await _eventHandler.Handle(orderCreatedEvent, default);

        payer.UserAddresses.Should().Contain(orderCreatedEvent.BillingAddress);
        payer.UserAddresses.Should().Contain(orderCreatedEvent.DeliveryAddress);

        _mockUserRepository.Verify(repo => repo.UpdateAsync(payer), Times.Once());
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once());
    }
}
