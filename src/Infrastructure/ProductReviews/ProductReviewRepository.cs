using Application.Common.Persistence.Repositories;
using Application.ProductReviews.Queries.Projections;

using Domain.ProductReviewAggregate;
using Domain.ProductReviewAggregate.ValueObjects;

using Infrastructure.Common.Persistence;

using SharedKernel.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ProductReviews;

internal sealed class ProductReviewRepository :
    BaseRepository<ProductReview, ProductReviewId>,
    IProductReviewRepository
{
    public ProductReviewRepository(IECommerceDbContext dbContext)
        : base(dbContext)
    {
    }

    public async Task<
        IReadOnlyList<ProductReviewDetailedProjection>
    > GetProductReviewsDetailedSatisfyingAsync(
        ISpecificationQuery<ProductReview> specification,
        CancellationToken cancellationToken = default
    )
    {
        return await Context.ProductReviews
            .Where(specification.Criteria)
            .Select(review => new ProductReviewDetailedProjection(
                review.Id,
                review.ProductId,
                review.Title,
                review.Content,
                review.StarRating,
                review.CreatedAt,
                review.UpdatedAt,
                Context.Users
                    .Where(user => user.Id == review.UserId)
                    .Select(user => new ProductReviewUserProjection(
                        user.Id,
                        user.Name
                    ))
                    .First()
            ))
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
