using Application.Common.Persistence.Repositories;
using Application.ProductFeedback.DTOs;

using Domain.ProductFeedbackAggregate.ValueObjects;
using DomainProductFeedback = Domain.ProductFeedbackAggregate.ProductFeedback;

using Infrastructure.Common.Persistence;

using SharedKernel.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ProductFeedback;

internal sealed class ProductFeedbackRepository :
    BaseRepository<DomainProductFeedback, ProductFeedbackId>,
    IProductFeedbackRepository
{
    public ProductFeedbackRepository(IECommerceDbContext dbContext)
        : base(dbContext)
    {
    }

    /// <inheritdoc/>
    public async Task<
        IEnumerable<ProductFeedbackDetailedResult>
    > GetProductFeedbackDetailedSatisfyingAsync(
        ISpecificationQuery<DomainProductFeedback> specification,
        CancellationToken cancellationToken = default
    )
    {
        return await Context.ProductFeedback
            .Where(specification.Criteria)
            .Select(productFeedback => new ProductFeedbackDetailedResult(
                productFeedback,
                Context.Users
                    .Where(user => user.Id == productFeedback.UserId)
                    .Select(user => new ProductFeedbackUserResult(
                        user.Id,
                        user.Name
                    ))
                    .First()
            ))
            .ToListAsync(cancellationToken);
    }

    public async Task<
        IEnumerable<ProductFeedbackResult>
    > GetProductFeedbackSatisfyingAsync(
        ISpecificationQuery<DomainProductFeedback> specification,
        CancellationToken cancellationToken = default
    )
    {
        return await Context.ProductFeedback
            .Where(specification.Criteria)
            .Select(productFeedback => new ProductFeedbackResult(productFeedback))
            .ToListAsync(cancellationToken);
    }
}
