using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Services;
using Domain.PaymentAggregate.Events;
using Domain.UserAggregate;
using MediatR;

namespace Application.Payments.Events;

/// <summary>
/// Handles the <see cref="PaymentCreated"/> event.
/// Verifies if the payer exists and also assign the delivery and billing to them.
/// Also, Initiates the authorization of the payment.
/// </summary>
public class PaymentCreatedHandler : INotificationHandler<PaymentCreated>
{
    private readonly IPaymentGateway _paymentGateway;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="PaymentCreatedHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="paymentGateway">The payment gateway.</param>
    public PaymentCreatedHandler(IUnitOfWork unitOfWork, IPaymentGateway paymentGateway)
    {
        _paymentGateway = paymentGateway;
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc/>
    public async Task Handle(PaymentCreated notification, CancellationToken cancellationToken)
    {
        var payer = await _unitOfWork.UserRepository.FindByIdAsync(notification.PayerId);

        if (payer == null || !payer.IsActive)
        {
            await CancelPayment(notification);
            return;
        }

        await UpdatePayerAddresses(notification, payer);

        _ = _paymentGateway.AuthorizePaymentAsync(
            payment: notification.Payment,
            payer: payer,
            deliveryAddress: notification.DeliveryAddress,
            billingAddress: notification.BillingAddress
        );
    }

    private async Task UpdatePayerAddresses(PaymentCreated notification, User payer)
    {
        payer.AssignAddress(notification.DeliveryAddress, notification.BillingAddress);

        await _unitOfWork.UserRepository.UpdateAsync(payer);

        await _unitOfWork.SaveChangesAsync();
    }

    private async Task CancelPayment(PaymentCreated notification)
    {
        notification.Payment.CancelPayment("Payer does not exist or is inactive");

        await _unitOfWork.PaymentRepository.UpdateAsync(notification.Payment);

        await _unitOfWork.SaveChangesAsync();
    }
}
