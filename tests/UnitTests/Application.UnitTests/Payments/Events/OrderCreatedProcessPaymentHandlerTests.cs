using Application.Payments.Events;
using Application.Common.Persistence.Repositories;
using Application.Common.Persistence;
using Application.Common.PaymentGateway;
using Application.Common.PaymentGateway.Requests;
using Application.UnitTests.TestUtils.Events.Orders;
using Application.UnitTests.TestUtils.PaymentGateway;

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
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IPaymentRepository> _mockPaymentRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;

    /// <summary>
    /// Initiates a new instance of the
    /// <see cref="OrderCreatedProcessPaymentHandlerTests"/> class.
    /// </summary>
    public OrderCreatedProcessPaymentHandlerTests()
    {
        _mockPaymentGateway = new Mock<IPaymentGateway>();
        _mockUserRepository = new Mock<IUserRepository>();
        _mockPaymentRepository = new Mock<IPaymentRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _eventHandler = new OrderCreatedProcessPaymentHandler(
            _mockPaymentGateway.Object,
            _mockUnitOfWork.Object,
            _mockPaymentRepository.Object,
            _mockUserRepository.Object
        );
    }

    /// <summary>
    /// Verifies the handler authorizes and creates a new payment.
    /// </summary>
    [Fact]
    public async Task HandleOrderCreated_WithValidOrder_AuthorizesAndCreatesPayment()
    {
        var payerId = UserId.Create(1);
        var payer = UserUtils.CreateCustomer(id: payerId);
        var order = await OrderUtils.CreateOrderAsync(ownerId: payerId);

        var paymentResponse = PaymentResponseUtils.CreateResponse();

        var notification = await OrderCreatedUtils.CreateEventAsync(
            order: order
        );

        _mockUserRepository
            .Setup(repo => repo.FindFirstSatisfyingAsync(
                It.IsAny<ISpecificationQuery<User>>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(payer);

        _mockPaymentGateway
            .Setup(g => g.AuthorizePaymentAsync(It.IsAny<AuthorizePaymentRequest>()))
            .ReturnsAsync(paymentResponse);

        await _eventHandler.Handle(notification, default);

        _mockPaymentGateway.Verify(
            g => g.AuthorizePaymentAsync(
                It.Is<AuthorizePaymentRequest>(
                    input => input.requestId == notification.RequestId
                    && input.order == order
                    && input.paymentMethod.Name == notification.PaymentMethod.Name
                    && input.payer == payer
                    && input.billingAddress == notification.BillingAddress
                    && input.installments == notification.Installments
                )
            ),
            Times.Once()
        );

        _mockPaymentRepository.Verify(
            r => r.AddAsync(It.Is<Payment>(p =>
                p.Id == paymentResponse.PaymentId
                && p.PaymentStatus == paymentResponse.Status
            )),
            Times.Once()
        );

        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once());
    }
}
