using Application.Common.Persistence.Repositories;

using Domain.PaymentAggregate;
using Domain.PaymentAggregate.ValueObjects;

using Infrastructure.Common.Persistence;

namespace Infrastructure.Payments;

internal sealed class PaymentRepository : BaseRepository<Payment, PaymentId>, IPaymentRepository
{
    public PaymentRepository(IECommerceDbContext dbContext) : base(dbContext)
    {
    }
}
