using Domain.ProductAggregate.ValueObjects;

using SharedKernel.Models;

using System.Linq.Expressions;

namespace Domain.ProductReviewAggregate.Specifications;

/// <summary>
/// Represents a query specification to retrieve the reviews of a specific product.
/// </summary>
public class QueryProductReviewsSpecification
    : CompositeQuerySpecification<ProductReview>
{
    private readonly ProductId _productId;

    /// <inheritdoc/>
    public override Expression<Func<ProductReview, bool>> Criteria =>
        review => review.ProductId == _productId;

    /// <summary>
    /// Initiates a new instance of the <see cref="QueryProductReviewsSpecification"/>
    /// class.
    /// </summary>
    /// <param name="productId">The product identifier.</param>
    public QueryProductReviewsSpecification(ProductId productId) : base()
    {
        _productId = productId;
    }
}
