using Application.Common.PaymentGateway;
using Application.Common.Persistence;

using Domain.OrderAggregate.Events;
using Domain.PaymentAggregate;
using Domain.PaymentAggregate.ValueObjects;
using Domain.UserAggregate.Specification;

using MediatR;

namespace Application.Payments.Events;

/// <summary>
/// Handles the <see cref="OrderCreated"/> event authorizing the order payment.
/// </summary>
public class OrderCreatedProcessPaymentHandler : INotificationHandler<OrderCreated>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPaymentGateway _paymentGateway;

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderCreatedProcessPaymentHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="paymentGateway">The payment gateway.</param>
    /// <param name="paymentRepository">The payment repository.</param>
    /// <param name="userRepository">The user repository.</param>
    public OrderCreatedProcessPaymentHandler(
        IPaymentGateway paymentGateway,
        IUnitOfWork unitOfWork,
        IPaymentRepository paymentRepository,
        IUserRepository userRepository
    )
    {
        _unitOfWork = unitOfWork;
        _paymentGateway = paymentGateway;
        _paymentRepository = paymentRepository;
        _userRepository = userRepository;
    }

    /// <inheritdoc/>
    public async Task Handle(OrderCreated notification, CancellationToken cancellationToken)
    {
        var payer = await _userRepository.FindFirstSatisfyingAsync(
            new QueryActiveUserByIdSpecification(notification.Order.OwnerId),
            cancellationToken
        );

        var response = await _paymentGateway.AuthorizePaymentAsync(new AuthorizePaymentInput(
            requestId: notification.RequestId,
            order: notification.Order,
            paymentMethod: notification.PaymentMethod,
            payer: payer,
            billingAddress: notification.BillingAddress,
            installments: notification.Installments
        ));

        var payment = Payment.Create(
            PaymentId.Create(response.PaymentId),
            notification.Order.Id,
            response.Status
        );

        await _paymentRepository.AddAsync(payment);
        await _unitOfWork.SaveChangesAsync();
    }
}
