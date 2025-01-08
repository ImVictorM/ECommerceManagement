using Application.Common.Errors;
using Application.Common.Interfaces.Payments;
using Application.Common.Interfaces.Persistence;

using Domain.OrderAggregate.Events;
using Domain.PaymentAggregate;
using Domain.PaymentAggregate.ValueObjects;
using Domain.UserAggregate.Specification;

using MediatR;

namespace Application.Orders.Events;

/// <summary>
/// Handles the <see cref="OrderCreated"/> event authorizing the order payment.
/// </summary>
public class OrderCreatedHandler : INotificationHandler<OrderCreated>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPaymentGateway _paymentGateway;

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderCreatedHandler"/> class.
    /// </summary>
    /// <param name="paymentGateway">The payment gateway.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    public OrderCreatedHandler(IPaymentGateway paymentGateway, IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _paymentGateway = paymentGateway;
    }

    /// <inheritdoc/>
    public async Task Handle(OrderCreated notification, CancellationToken cancellationToken)
    {
        var payer = await _unitOfWork.UserRepository
            .FindFirstSatisfyingAsync(new QueryActiveUserByIdSpecification(notification.Order.OwnerId))
            ?? throw new UserNotFoundException($"The order payer with id {notification.Order.OwnerId} could not be found");

        payer.AssignAddress(notification.DeliveryAddress, notification.BillingAddress);

        var response = await _paymentGateway.AuthorizePaymentAsync(
            requestId: notification.RequestId,
            order: notification.Order,
            paymentMethod: notification.PaymentMethod,
            payer: payer,
            deliveryAddress: notification.DeliveryAddress,
            billingAddress: notification.BillingAddress,
            installments: notification.Installments
        );

        var payment = Payment.Create(
            PaymentId.Create(response.PaymentId),
            notification.Order.Id,
            response.Status
        );

        await _unitOfWork.PaymentRepository.AddAsync(payment);
        await _unitOfWork.SaveChangesAsync();
    }
}
