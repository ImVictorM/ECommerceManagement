using Application.Common.PaymentGateway;
using Application.Common.Persistence;

using Domain.PaymentAggregate.Events;

using MediatR;

namespace Application.Payments.Events;

/// <summary>
/// Handles the <see cref="PaymentAuthorized"/> event by
/// capturing the payment.
/// </summary>
public sealed class PaymentAuthorizedCapturePaymentHandler : INotificationHandler<PaymentAuthorized>
{
    private readonly IPaymentGateway _paymentGateway;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="PaymentAuthorizedCapturePaymentHandler"/> class.
    /// </summary>
    /// <param name="paymentGateway">The payment gateway.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="paymentRepository">The payment repository.</param>
    public PaymentAuthorizedCapturePaymentHandler(
        IUnitOfWork unitOfWork,
        IPaymentGateway paymentGateway,
        IPaymentRepository paymentRepository
    )
    {
        _paymentGateway = paymentGateway;
        _paymentRepository = paymentRepository;
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc/>
    public async Task Handle(PaymentAuthorized notification, CancellationToken cancellationToken)
    {
        var payment = notification.Payment;

        var captureResponse = await _paymentGateway.CapturePaymentAsync(payment.Id.ToString());

        payment.UpdatePaymentStatus(captureResponse.Status);

        _paymentRepository.Update(payment);

        await _unitOfWork.SaveChangesAsync();
    }
}
