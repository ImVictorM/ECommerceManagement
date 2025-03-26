using Domain.ProductAggregate.ValueObjects;

using SharedKernel.Models;

using System.Linq.Expressions;

namespace Domain.ProductFeedbackAggregate.Specifications;

/// <summary>
/// Represents a query specification to retrieve feedback for the product
/// with the specified identifier.
/// </summary>
public class QueryFeedbackForProduct
    : CompositeQuerySpecification<ProductFeedback>
{
    private readonly ProductId _productId;

    /// <inheritdoc/>
    public override Expression<Func<ProductFeedback, bool>> Criteria =>
        feedback => feedback.ProductId == _productId;

    /// <summary>
    /// Initiates a new instance of the <see cref="QueryFeedbackForProduct"/>
    /// class.
    /// </summary>
    /// <param name="productId">The product identifier.</param>
    public QueryFeedbackForProduct(ProductId productId) : base()
    {
        _productId = productId;
    }
}
