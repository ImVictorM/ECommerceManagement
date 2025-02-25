using Application.Common.PaymentGateway;
using Application.Common.Persistence;
using Application.Common.Persistence.Repositories;

using Domain.PaymentAggregate.Events;

using MediatR;

namespace Application.Payments.Events;

internal sealed class PaymentRejectedCancelAuthorizationHandler : INotificationHandler<PaymentRejected>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPaymentGateway _paymentGateway;
    private readonly IPaymentRepository _paymentRepository;

    public PaymentRejectedCancelAuthorizationHandler(
        IUnitOfWork unitOfWork,
        IPaymentGateway paymentGateway,
        IPaymentRepository paymentRepository
    )
    {
        _unitOfWork = unitOfWork;
        _paymentGateway = paymentGateway;
        _paymentRepository = paymentRepository;
    }

    /// <inheritdoc/>
    public async Task Handle(PaymentRejected notification, CancellationToken cancellationToken)
    {
        var payment = notification.Payment;

        var response = await _paymentGateway.CancelAuthorizationAsync(payment.Id.ToString());

        payment.UpdatePaymentStatus(response.Status);

        _paymentRepository.Update(payment);

        await _unitOfWork.SaveChangesAsync();
    }
}
