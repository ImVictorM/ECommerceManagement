using Application.Common.Interfaces.Persistence;
using Domain.PaymentAggregate.Events;
using MediatR;

namespace Application.Payments.Events;

/// <summary>
/// Handles <see cref="PaymentCanceled"/> event canceling the order this payment references.
/// </summary>
public class PaymentCanceledHandler : INotificationHandler<PaymentCanceled>
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="PaymentCanceledHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    public PaymentCanceledHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc/>
    public async Task Handle(PaymentCanceled notification, CancellationToken cancellationToken)
    {
        var orderToCancel = await _unitOfWork.OrderRepository.FindByIdAsync(notification.payment.OrderId);

        if (orderToCancel == null)
        {
            return;
        }

        orderToCancel.CancelOrder("Payment canceled");

        await _unitOfWork.OrderRepository.UpdateAsync(orderToCancel);

        await _unitOfWork.SaveChangesAsync();
    }
}
