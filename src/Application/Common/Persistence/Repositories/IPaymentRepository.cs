using Domain.PaymentAggregate;
using Domain.PaymentAggregate.ValueObjects;

namespace Application.Common.Persistence.Repositories;

/// <summary>
/// Defines the contract for payment persistence operations.
/// </summary>
public interface IPaymentRepository : IBaseRepository<Payment, PaymentId>
{
}
