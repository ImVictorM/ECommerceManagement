using Domain.ProductReviewAggregate.ValueObjects;
using Domain.ProductReviewAggregate;

using Application.ProductReviews.Queries.Projections;

using SharedKernel.Interfaces;

namespace Application.Common.Persistence.Repositories;

/// <summary>
/// Defines the contract for product review persistence operations.
/// </summary>
public interface IProductReviewRepository
    : IBaseRepository<ProductReview, ProductReviewId>
{
    /// <summary>
    /// Retrieves detailed product reviews satisfying the specified specification.
    /// </summary>
    /// <param name="specification">The specification</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A list of <see cref="ProductReviewDetailedProjection"/>.</returns>
    Task<IReadOnlyList<
        ProductReviewDetailedProjection
    >> GetProductReviewsDetailedSatisfyingAsync(
        ISpecificationQuery<ProductReview> specification,
        CancellationToken cancellationToken = default
    );
}
