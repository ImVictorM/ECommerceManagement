using Application.Common.Persistence;

using Domain.PaymentAggregate.Events;

using MediatR;

namespace Application.Orders.Events;

/// <summary>
/// Handles the <see cref="PaymentCanceled"/> event by
/// canceling the related order.
/// </summary>
public sealed class PaymentCanceledCancelOrderHandler : INotificationHandler<PaymentCanceled>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOrderRepository _orderRepository;

    /// <summary>
    /// Initiates a new instance of the <see cref="PaymentCanceledCancelOrderHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">the unit of work.</param>
    /// <param name="orderRepository">The order repository.</param>
    public PaymentCanceledCancelOrderHandler(
        IUnitOfWork unitOfWork,
        IOrderRepository orderRepository
    )
    {
        _unitOfWork = unitOfWork;
        _orderRepository = orderRepository;
    }

    /// <inheritdoc/>
    public async Task Handle(PaymentCanceled notification, CancellationToken cancellationToken)
    {
        var orderId = notification.Payment.OrderId;

        var order = await _orderRepository.FindByIdAsync(orderId, cancellationToken);

        if (order != null)
        {
            order.Cancel("The payment was canceled");

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
