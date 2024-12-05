using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Services;
using Domain.OrderAggregate;
using Domain.OrderAggregate.Events;
using Domain.PaymentAggregate;
using MediatR;

namespace Application.Orders.Events;

/// <summary>
/// Handles the <see cref="OrderCreated"/> event.
/// </summary>
public class OrderCreatedHandler : INotificationHandler<OrderCreated>
{
    private readonly IPaymentGateway _paymentGateway;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderCreatedHandler"/> class.
    /// </summary>
    /// <param name="paymentGateway">The payment gateway.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    public OrderCreatedHandler(IPaymentGateway paymentGateway, IUnitOfWork unitOfWork)
    {
        _paymentGateway = paymentGateway;
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc/>
    public async Task Handle(OrderCreated notification, CancellationToken cancellationToken)
    {
        var order = notification.Order;

        var orderOwner = await _unitOfWork.UserRepository.FindByIdAsync(notification.Order.UserId);

        if (orderOwner == null || !orderOwner.IsActive)
        {
            await CancelOrderAsync(order, "The order owner does not exist or is inactive");
            return;
        }

        orderOwner.AddAddress(notification.BillingAddress, notification.DeliveryAddress);

        var payment = Payment.Create(
            order.CalculateTotalApplyingDiscounts(),
            order.Id,
            notification.PaymentMethod,
            notification.Installments
        );

        await _unitOfWork.PaymentRepository.AddAsync(payment);
        await _unitOfWork.UserRepository.UpdateAsync(orderOwner);

        await _unitOfWork.SaveChangesAsync();

        _ = _paymentGateway.AuthorizePaymentAsync(
            payment: payment,
            payer: orderOwner,
            deliveryAddress: notification.DeliveryAddress,
            billingAddress: notification.BillingAddress
        );
    }

    private async Task CancelOrderAsync(Order order, string reason)
    {
        order.CancelOrder(reason);

        await _unitOfWork.OrderRepository.UpdateAsync(order);

        await _unitOfWork.SaveChangesAsync();
    }
}
