using Application.Common.Errors;
using Application.Common.Persistence;
using Application.Common.Persistence.Repositories;

using Domain.PaymentAggregate.Events;

using MediatR;

namespace Application.Orders.Events;

internal sealed class PaymentApprovedMarkOrderAsPaidHandler : INotificationHandler<PaymentApproved>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

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
