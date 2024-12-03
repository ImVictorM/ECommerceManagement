using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Services;
using Domain.OrderAggregate.Events;
using Domain.PaymentAggregate;
using MediatR;

namespace Application.Orders.Commands.Events;

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
    /// <param name="paymentGateway"></param>
    /// <param name="unitOfWork"></param>
    public OrderCreatedHandler(IPaymentGateway paymentGateway, IUnitOfWork unitOfWork)
    {
        _paymentGateway = paymentGateway;
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc/>
    public async Task Handle(OrderCreated notification, CancellationToken cancellationToken)
    {
        // 1. Create Payment
        var payment = Payment.Create(
            notification.Order.GetPriceAfterDiscounts(),
            notification.Order.Id,
            notification.PaymentMethod,
            notification.Installments
        );

        await _unitOfWork.PaymentRepository.AddAsync(payment);

        await _unitOfWork.SaveChangesAsync();

        _ = _paymentGateway.AuthorizePaymentAsync(payment);
    }
}
