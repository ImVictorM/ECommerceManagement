using Application.Payments.Events;
using Application.UnitTests.TestUtils.Events.Orders;
using Application.Common.Persistence;
using Application.Common.PaymentGateway;
using Application.UnitTests.TestUtils.PaymentGateway;

using Domain.PaymentAggregate.ValueObjects;
using Domain.PaymentAggregate;
using Domain.UnitTests.TestUtils;
using Domain.UserAggregate.ValueObjects;
using Domain.UserAggregate;

using SharedKernel.Interfaces;

using Moq;

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
        var payer = UserUtils.CreateCustomer(id: payerId);
        var order = await OrderUtils.CreateOrderAsync(ownerId: payerId);

        var paymentResponse = PaymentResponseUtils.CreateResponse();

        var orderCreatedEvent = await OrderCreatedUtils.CreateEventAsync(order: order);

        _mockUserRepository
            .Setup(repo => repo.FindFirstSatisfyingAsync(It.IsAny<ISpecificationQuery<User>>()))
            .ReturnsAsync(payer);

        _mockPaymentGateway
            .Setup(g => g.AuthorizePaymentAsync(It.IsAny<AuthorizePaymentInput>()))
            .ReturnsAsync(paymentResponse);

        await _eventHandler.Handle(orderCreatedEvent, default);

        _mockPaymentGateway.Verify(
            g => g.AuthorizePaymentAsync(
                It.Is<AuthorizePaymentInput>(
                    input => input.requestId == orderCreatedEvent.RequestId
                    && input.order == order
                    && input.paymentMethod.Type == orderCreatedEvent.PaymentMethod.Type
                    && input.payer == payer
                    && input.billingAddress == orderCreatedEvent.BillingAddress
                    && input.installments == orderCreatedEvent.Installments
                )
            ),
            Times.Once()
        );

        _mockPaymentRepository.Verify(
            r => r.AddAsync(It.Is<Payment>(p =>
                p.Id.ToString() == paymentResponse.PaymentId
                && p.PaymentStatusId == paymentResponse.Status.Id
            )),
            Times.Once()
        );

        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once());
    }
}
