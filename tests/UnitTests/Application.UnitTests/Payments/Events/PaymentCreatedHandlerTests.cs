using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Services;
using Application.Payments.Events;
using Application.UnitTests.Payments.Events.TestUtils;
using Domain.PaymentAggregate;
using Domain.PaymentAggregate.Enumeration;
using Domain.PaymentAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;
using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;
using FluentAssertions;
using Moq;
using SharedKernel.ValueObjects;

namespace Application.UnitTests.Payments.Events;

/// <summary>
/// Tests for the <see cref="PaymentCreatedHandler"/> event handler.
/// </summary>
public class PaymentCreatedHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IPaymentGateway> _mockPaymentGateway;
    private readonly Mock<IRepository<User, UserId>> _mockUserRepository;
    private readonly Mock<IRepository<Payment, PaymentId>> _mockPaymentRepository;
    private readonly PaymentCreatedHandler _eventHandler;

    /// <summary>
    /// Initiates a new instance of the <see cref="PaymentCreatedHandlerTests"/> class.
    /// </summary>
    public PaymentCreatedHandlerTests()
    {
        _mockPaymentGateway = new Mock<IPaymentGateway>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockPaymentRepository = new Mock<IRepository<Payment, PaymentId>>();
        _mockUserRepository = new Mock<IRepository<User, UserId>>();

        _mockUnitOfWork.Setup(uow => uow.PaymentRepository).Returns(_mockPaymentRepository.Object);
        _mockUnitOfWork.Setup(uow => uow.UserRepository).Returns(_mockUserRepository.Object);

        _eventHandler = new PaymentCreatedHandler(_mockUnitOfWork.Object, _mockPaymentGateway.Object);
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
    /// Tests when the event is fired the user addresses is updated and the authorization process for the payment is initiated.
    /// </summary>
    [Fact]
    public async Task HandlePaymentCreated_WhenEventIsFiredAndPayerIsValid_TriggersAuthorizationProcess()
    {
        var payer = UserUtils.CreateUser();
        var paymentCreatedEvent = PaymentCreatedUtils.CreateEvent();

        _mockUserRepository.Setup(r => r.FindByIdAsync(It.IsAny<UserId>())).ReturnsAsync(payer);

        await _eventHandler.Handle(paymentCreatedEvent, default);

        payer.UserAddresses.Should().Contain(paymentCreatedEvent.BillingAddress);
        payer.UserAddresses.Should().Contain(paymentCreatedEvent.DeliveryAddress);

        _mockUserRepository.Verify(r => r.UpdateAsync(payer), Times.Once());
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once());
        _mockPaymentGateway.Verify(
             g => g.AuthorizePaymentAsync(
                 paymentCreatedEvent.Payment,
                 payer,
                 paymentCreatedEvent.BillingAddress,
                 paymentCreatedEvent.DeliveryAddress
             ),
             Times.Once()
        );
    }

    /// <summary>
    /// Tests when the payer is not found or is inactive the payment is canceled, updating the payment status and description.
    /// Also, verifies if the authorize process is not initiated.
    /// </summary>
    /// <param name="userNotFound">The inactive or null user.</param>
    [Theory]
    [MemberData(nameof(NotFoundUser))]
    public async Task HandlePaymentCreated_WhenUserCouldNotBeFound_CancelsPayment(User? userNotFound)
    {
        _mockUserRepository.Setup(r => r.FindByIdAsync(It.IsAny<UserId>())).ReturnsAsync(userNotFound);

        var paymentCreatedEvent = PaymentCreatedUtils.CreateEvent();

        await _eventHandler.Handle(paymentCreatedEvent, default);

        paymentCreatedEvent.Payment.PaymentStatusId.Should().Be(PaymentStatus.Canceled.Id);
        paymentCreatedEvent.Payment.Description.Should().Be("Payer does not exist or is inactive");

        _mockPaymentRepository.Verify(r => r.UpdateAsync(paymentCreatedEvent.Payment), Times.Once());
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once());

        _mockPaymentGateway.Verify(
            g => g.AuthorizePaymentAsync(
                It.IsAny<Payment>(),
                It.IsAny<User?>(),
                It.IsAny<Address?>(),
                It.IsAny<Address?>()
            ),
            Times.Never()
        );

    }
}
