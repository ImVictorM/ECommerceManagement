using Application.Common.Persistence;
using Application.Payments.Common.Errors;

using Domain.PaymentAggregate.Enumerations;
using Domain.PaymentAggregate.ValueObjects;

using SharedKernel.Models;

using MediatR;

namespace Application.Payments.Commands.UpdatePaymentStatus;

/// <summary>
/// Handles the <see cref="UpdatePaymentStatusCommand"/> command.
/// </summary>
public class UpdatePaymentStatusCommandHandler : IRequestHandler<UpdatePaymentStatusCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="UpdatePaymentStatusCommand"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    public UpdatePaymentStatusCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc/>
    public async Task<Unit> Handle(UpdatePaymentStatusCommand request, CancellationToken cancellationToken)
    {
        var paymentId = PaymentId.Create(request.PaymentId);
        var paymentStatus = BaseEnumeration.FromDisplayName<PaymentStatus>(request.Status);

        var payment =
            await _unitOfWork.PaymentRepository.FindByIdAsync(paymentId)
            ?? throw new PaymentNotFoundException();

        payment.UpdatePaymentStatus(paymentStatus);

        await _unitOfWork.SaveChangesAsync();

        return Unit.Value;
    }
}
