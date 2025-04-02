using SharedKernel.Specifications;

namespace Domain.ProductReviewAggregate.Specifications;

/// <summary>
/// Query specification to retrieve active product reviews.
/// </summary>
public class QueryActiveProductReviewsSpecification
    : QueryActiveSpecification<ProductReview>
{
}
