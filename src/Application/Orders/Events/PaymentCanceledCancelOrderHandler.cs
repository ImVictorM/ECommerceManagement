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

    /// <summary>
    /// Initiates a new instance of the <see cref="PaymentCanceledCancelOrderHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">the unit of work.</param>
    public PaymentCanceledCancelOrderHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc/>
    public async Task Handle(PaymentCanceled notification, CancellationToken cancellationToken)
    {
        var orderId = notification.Payment.OrderId;

        var order = await _unitOfWork.OrderRepository.FindByIdAsync(orderId);

        if (order != null)
        {
            order.Cancel("The payment was canceled");

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
