using Application.Common.PaymentGateway;
using Application.Common.Persistence;
using Application.Common.Persistence.Repositories;

using Domain.PaymentAggregate.Events;

using MediatR;

namespace Application.Payments.Events;

internal sealed class PaymentAuthorizedCapturePaymentHandler : INotificationHandler<PaymentAuthorized>
{
    private readonly IPaymentGateway _paymentGateway;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IUnitOfWork _unitOfWork;

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
