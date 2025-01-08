using Application.Common.Interfaces.Payments;
using Application.Common.Interfaces.Persistence;
using Application.Orders.Common.Errors;

using Domain.PaymentAggregate;
using Domain.PaymentAggregate.Enumerations;
using Domain.PaymentAggregate.Events;

using MediatR;

namespace Application.Payments.Events;

/// <summary>
/// Handles the <see cref="PaymentStatusChanged"/> event.
/// </summary>
public class PaymentStatusChangedHandler : INotificationHandler<PaymentStatusChanged>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPaymentGateway _paymentGateway;

    /// <summary>
    /// Initiates a new instance of the <see cref="PaymentStatusChangedHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="paymentGateway">The payment gateway.</param>
    public PaymentStatusChangedHandler(
        IUnitOfWork unitOfWork,
        IPaymentGateway paymentGateway
    )
    {
        _unitOfWork = unitOfWork;
        _paymentGateway = paymentGateway;
    }

    /// <inheritdoc/>
    public async Task Handle(PaymentStatusChanged notification, CancellationToken cancellationToken)
    {
        var payment = notification.Payment;

        if (payment.PaymentStatusId == PaymentStatus.Authorized.Id)
        {
            await CapturePaymentAsync(payment);
        }
        else if (payment.PaymentStatusId == PaymentStatus.Approved.Id)
        {
            await ApproveOrderAsync(payment);
        }
        else if (payment.PaymentStatusId == PaymentStatus.Rejected.Id)
        {
            await RejectPaymentAsync(payment);
        }
        else if (payment.PaymentStatusId == PaymentStatus.Canceled.Id)
        {
            await CancelOrderAsync(payment);
        }

        await _unitOfWork.SaveChangesAsync();
    }

    private async Task CapturePaymentAsync(Payment payment)
    {
        var response = await _paymentGateway.CapturePaymentAsync(payment.Id);

        payment.UpdatePaymentStatus(response.Status);
    }

    private async Task ApproveOrderAsync(Payment payment)
    {
        var paymentWithDetails = await _paymentGateway.GetPaymentByIdAsync(payment.Id);

        var order = await _unitOfWork.OrderRepository.FindByIdAsync(payment.OrderId) ??
                    throw new OrderNotFoundException("The payment order could not be found");

        order.MarkAsPaid(paymentWithDetails.DeliveryAddress);
    }

    private async Task RejectPaymentAsync(Payment payment)
    {
        var response = await _paymentGateway.CancelAuthorizationAsync(payment.Id);

        payment.UpdatePaymentStatus(response.Status);
    }

    private async Task CancelOrderAsync(Payment payment)
    {
        var order = await _unitOfWork.OrderRepository.FindByIdAsync(payment.OrderId) ??
                    throw new OrderNotFoundException("The payment order could not be found");

        order.Cancel("The payment was canceled");
    }
}
