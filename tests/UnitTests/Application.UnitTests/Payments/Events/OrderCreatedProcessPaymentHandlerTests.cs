using Application.Payments.Events;
using Application.UnitTests.TestUtils.Events.Orders;

using Domain.OrderAggregate;
using Domain.PaymentAggregate.Enumerations;
using Domain.PaymentAggregate.ValueObjects;
using Domain.PaymentAggregate;
using Domain.UnitTests.TestUtils;
using Domain.UserAggregate.ValueObjects;
using Domain.UserAggregate;

using SharedKernel.Interfaces;
using SharedKernel.ValueObjects;

using Moq;
using Application.Common.Persistence;
using Application.Common.PaymentGateway;

namespace Application.UnitTests.Payments.Events;

/// <summary>
/// Unit tests for the <see cref="OrderCreatedProcessPaymentHandler"/> handler.
/// </summary>
public class OrderCreatedProcessPaymentHandlerTests
{
    private readonly OrderCreatedProcessPaymentHandler _eventHandler;
    private readonly Mock<IPaymentGateway> _mockPaymentGateway;
    private readonly Mock<IRepository<User, UserId>> _mockUserRepository;
    private readonly Mock<IRepository<Payment, PaymentId>> _mockPaymentRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderCreatedProcessPaymentHandlerTests"/> class.
    /// </summary>
    public OrderCreatedProcessPaymentHandlerTests()
    {
        _mockPaymentGateway = new Mock<IPaymentGateway>();
        _mockUserRepository = new Mock<IRepository<User, UserId>>();
        _mockPaymentRepository = new Mock<IRepository<Payment, PaymentId>>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _mockUnitOfWork.Setup(uow => uow.PaymentRepository).Returns(_mockPaymentRepository.Object);
        _mockUnitOfWork.Setup(uow => uow.UserRepository).Returns(_mockUserRepository.Object);

        _eventHandler = new OrderCreatedProcessPaymentHandler(_mockPaymentGateway.Object, _mockUnitOfWork.Object);
    }

    /// <summary>
    /// Tests the handler authorizes and creates a new payment.
    /// </summary>
    [Fact]
    public async Task HandleOrderCreated_WhenEventIsFired_AuthorizesAndCreatesPayment()
    {
        var payerId = UserId.Create(1);
        var payer = UserUtils.CreateUser(id: payerId);
        var order = await OrderUtils.CreateOrderAsync(ownerId: payerId);

        var mockPaymentResponse = new Mock<PaymentResponse>();
        mockPaymentResponse.SetupGet(x => x.PaymentId).Returns(Guid.NewGuid().ToString());
        mockPaymentResponse.SetupGet(x => x.Status).Returns(PaymentStatus.Pending);

        var orderCreatedEvent = await OrderCreatedUtils.CreateEventAsync(order: order);

        _mockUserRepository
            .Setup(repo => repo.FindFirstSatisfyingAsync(It.IsAny<ISpecificationQuery<User>>()))
            .ReturnsAsync(payer);

        _mockPaymentGateway
            .Setup(g => g.AuthorizePaymentAsync(
                It.IsAny<Guid>(),
                It.IsAny<Order>(),
                It.IsAny<IPaymentMethod>(),
                It.IsAny<User?>(),
                It.IsAny<Address?>(),
                It.IsAny<int?>()
            ))
            .ReturnsAsync(mockPaymentResponse.Object);

        await _eventHandler.Handle(orderCreatedEvent, default);

        _mockPaymentGateway.Verify(
            g => g.AuthorizePaymentAsync(
                It.Is<Guid>(r => r == orderCreatedEvent.RequestId),
                It.Is<Order>(o => o == order),
                It.Is<IPaymentMethod>(pm => pm.Type == orderCreatedEvent.PaymentMethod.Type),
                It.Is<User>(u => u == payer),
                It.Is<Address>(address => address == orderCreatedEvent.BillingAddress),
                It.Is<int?>(i => i == orderCreatedEvent.Installments)
            ),
            Times.Once()
        );

        _mockPaymentRepository.Verify(
            r => r.AddAsync(It.Is<Payment>(p =>
                p.Id.ToString() == mockPaymentResponse.Object.PaymentId
                && p.PaymentStatusId == mockPaymentResponse.Object.Status.Id
            )),
            Times.Once()
        );

        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once());
    }
}
