using Application.Common.Persistence;
using Application.Common.Persistence.Repositories;

using Domain.PaymentAggregate.Events;

using MediatR;

namespace Application.Orders.Events;

internal sealed class PaymentCanceledCancelOrderHandler
    : INotificationHandler<PaymentCanceled>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOrderRepository _orderRepository;

    public PaymentCanceledCancelOrderHandler(
        IUnitOfWork unitOfWork,
        IOrderRepository orderRepository
    )
    {
        _unitOfWork = unitOfWork;
        _orderRepository = orderRepository;
    }

    public async Task Handle(
        PaymentCanceled notification,
        CancellationToken cancellationToken
    )
    {
        var orderId = notification.Payment.OrderId;

        var order = await _orderRepository.FindByIdAsync(orderId, cancellationToken);

        if (order == null)
        {
            return;
        }

        order.Cancel("The payment was canceled");

        await _unitOfWork.SaveChangesAsync();
    }
}
