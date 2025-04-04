using Application.Common.PaymentGateway;
using Application.Common.PaymentGateway.Requests;
using Application.Common.Persistence;
using Application.Common.Persistence.Repositories;

using Domain.OrderAggregate.Events;
using Domain.PaymentAggregate;
using Domain.UserAggregate.Specification;

using MediatR;

namespace Application.Payments.Events;

internal sealed class OrderCreatedProcessPaymentHandler
    : INotificationHandler<OrderCreated>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPaymentGateway _paymentGateway;

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

    public async Task Handle(
        OrderCreated notification,
        CancellationToken cancellationToken
    )
    {
        var payer = await _userRepository.FindFirstSatisfyingAsync(
            new QueryActiveUserByIdSpecification(notification.Order.OwnerId),
            cancellationToken
        );

        var authorizePaymentRequest = new AuthorizePaymentRequest(
            requestId: notification.RequestId,
            order: notification.Order,
            paymentMethod: notification.PaymentMethod,
            payer: payer,
            billingAddress: notification.BillingAddress,
            installments: notification.Installments
        );

        var response = await _paymentGateway
            .AuthorizePaymentAsync(authorizePaymentRequest);

        var payment = Payment.Create(
            response.PaymentId,
            notification.Order.Id,
            response.Status
        );

        await _paymentRepository.AddAsync(payment);
        await _unitOfWork.SaveChangesAsync();
    }
}
