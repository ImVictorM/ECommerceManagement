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
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="PaymentApprovedMarkOrderAsPaidHandler"/> class.
    /// </summary>
    /// <param name="orderRepository">The order repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    public PaymentApprovedMarkOrderAsPaidHandler(
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork
    )
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc/>
    public async Task Handle(PaymentApproved notification, CancellationToken cancellationToken)
    {
        var orderId = notification.Payment.OrderId;

        var order = await _orderRepository.FindByIdAsync(orderId, cancellationToken) ??
                    throw new OperationProcessFailedException($"The order with id {orderId} cannot be marked as paid because it does not exist");

        order.MarkAsPaid();

        await _unitOfWork.SaveChangesAsync();
    }
}
