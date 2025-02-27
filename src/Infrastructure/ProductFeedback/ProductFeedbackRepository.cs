using Application.Common.Persistence.Repositories;

using Domain.ProductFeedbackAggregate.ValueObjects;
using DomainProductFeedback = Domain.ProductFeedbackAggregate.ProductFeedback;

using Infrastructure.Common.Persistence;

namespace Infrastructure.ProductFeedback;

internal sealed class ProductFeedbackRepository
    : BaseRepository<DomainProductFeedback, ProductFeedbackId>, IProductFeedbackRepository
{
    public ProductFeedbackRepository(IECommerceDbContext dbContext) : base(dbContext)
    {
    }
}
