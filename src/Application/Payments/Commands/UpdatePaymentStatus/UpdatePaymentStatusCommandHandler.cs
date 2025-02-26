using Application.Payments.Errors;
using Application.Common.Persistence.Repositories;
using Application.Common.Persistence;

using Domain.PaymentAggregate.Enumerations;
using Domain.PaymentAggregate.ValueObjects;

using SharedKernel.Models;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Payments.Commands.UpdatePaymentStatus;

internal sealed partial class UpdatePaymentStatusCommandHandler : IRequestHandler<UpdatePaymentStatusCommand, Unit>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePaymentStatusCommandHandler(
        IUnitOfWork unitOfWork,
        IPaymentRepository paymentRepository,
        ILogger<UpdatePaymentStatusCommandHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _paymentRepository = paymentRepository;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<Unit> Handle(UpdatePaymentStatusCommand request, CancellationToken cancellationToken)
    {
        LogInitiatingPaymentStatusUpdate(request.PaymentId);

        var paymentId = PaymentId.Create(request.PaymentId);
        var paymentStatus = BaseEnumeration.FromDisplayName<PaymentStatus>(request.Status);

        var payment = await _paymentRepository.FindByIdAsync(paymentId, cancellationToken);

        if (payment == null)
        {
            LogPaymentNotFound();
            throw new PaymentNotFoundException();
        }

        LogUpdatingPaymentStatus(payment.PaymentStatus.Name);

        payment.UpdatePaymentStatus(paymentStatus);

        await _unitOfWork.SaveChangesAsync();

        LogPaymentUpdatedSuccessfully(payment.PaymentStatus.Name);

        return Unit.Value;
    }
}
