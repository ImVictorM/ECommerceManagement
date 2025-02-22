using Application.Common.Persistence;

using Domain.PaymentAggregate;
using Domain.PaymentAggregate.ValueObjects;

using Infrastructure.Common.Persistence;

namespace Infrastructure.Payments;

/// <summary>
/// Defines the implementation for payment persistence operations.
/// </summary>
public sealed class PaymentRepository : BaseRepository<Payment, PaymentId>, IPaymentRepository
{
    /// <summary>
    /// Initiates a new instance of the <see cref="PaymentRepository"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public PaymentRepository(ECommerceDbContext dbContext) : base(dbContext)
    {
    }
}
