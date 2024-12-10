using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Services;
using Domain.OrderAggregate.Events;
using Domain.PaymentAggregate;
using MediatR;

namespace Application.Orders.Events;

/// <summary>
/// Handles the <see cref="OrderCreated"/> event generating a payment for the order.
/// </summary>
public class OrderCreatedHandler : INotificationHandler<OrderCreated>
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderCreatedHandler"/> class.
    /// </summary>
    /// <param name="paymentGateway">The payment gateway.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    public OrderCreatedHandler(IPaymentGateway paymentGateway, IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc/>
    public async Task Handle(OrderCreated notification, CancellationToken cancellationToken)
    {
        var payment = Payment.Create(
            notification.Order.CalculateTotalApplyingDiscounts(),
            notification.Order.Id,
            notification.Order.OwnerId,
            notification.PaymentMethod,
            notification.BillingAddress,
            notification.DeliveryAddress,
            notification.Installments
        );

        await _unitOfWork.PaymentRepository.AddAsync(payment);

        await _unitOfWork.SaveChangesAsync();
    }
}
