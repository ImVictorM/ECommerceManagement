using Application.ProductReviews.DTOs.Results;

using MediatR;

namespace Application.ProductReviews.Queries.GetProductReviews;

/// <summary>
/// Represents a query to retrieve active reviews for the specified product.
/// </summary>
/// <param name="ProductId">The product identifier.</param>
public record GetProductReviewsQuery(string ProductId)
    : IRequest<IReadOnlyList<ProductReviewDetailedResult>>;
