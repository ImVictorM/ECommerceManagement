using Application.Common.PaymentGateway;
using Application.Common.Persistence;
using Domain.PaymentAggregate.Events;

using MediatR;

namespace Application.Payments.Events;

/// <summary>
/// Handles the <see cref="PaymentRejected"/> event by
/// cancelling the authorization process.
/// </summary>
public sealed class PaymentRejectedCancelAuthorizationHandler : INotificationHandler<PaymentRejected>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPaymentGateway _paymentGateway;

    /// <summary>
    /// Initiates a new instance of the <see cref="PaymentRejectedCancelAuthorizationHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="paymentGateway">The payment gateway.</param>
    public PaymentRejectedCancelAuthorizationHandler(IUnitOfWork unitOfWork, IPaymentGateway paymentGateway)
    {
        _unitOfWork = unitOfWork;
        _paymentGateway = paymentGateway;
    }

    /// <inheritdoc/>
    public async Task Handle(PaymentRejected notification, CancellationToken cancellationToken)
    {
        var payment = notification.Payment;

        var response = await _paymentGateway.CancelAuthorizationAsync(payment.Id.ToString());

        payment.UpdatePaymentStatus(response.Status);

        await _unitOfWork.SaveChangesAsync();
    }
}
