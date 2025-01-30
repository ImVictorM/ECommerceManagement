using Application.Common.Errors;
using Application.Common.Persistence;

using Domain.PaymentAggregate.Events;

using MediatR;

namespace Application.Orders.Events;

/// <summary>
/// Handles the <see cref="PaymentApproved"/> event by
/// marking the order as paid.
/// </summary>
public sealed class PaymentApprovedMarkOrderAsPaidHandler : INotificationHandler<PaymentApproved>
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="PaymentApprovedMarkOrderAsPaidHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    public PaymentApprovedMarkOrderAsPaidHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc/>
    public async Task Handle(PaymentApproved notification, CancellationToken cancellationToken)
    {
        var orderId = notification.Payment.OrderId;

        var order = await _unitOfWork.OrderRepository.FindByIdAsync(orderId) ??
                    throw new OperationProcessFailedException($"The order with id {orderId} cannot be marked as paid because it does not exist");

        order.MarkAsPaid();

        await _unitOfWork.SaveChangesAsync();
    }
}
