using Domain.UserAggregate.ValueObjects;

using SharedKernel.Models;

using System.Linq.Expressions;

namespace Domain.ProductReviewAggregate.Specifications;

/// <summary>
/// Represents a query to retrieve the reviews of an specific user.
/// </summary>
public class QueryUserReviewsSpecification
    : CompositeQuerySpecification<ProductReview>
{
    private readonly UserId _userId;

    /// <inheritdoc/>
    public override Expression<Func<ProductReview, bool>> Criteria =>
        feedback => feedback.UserId == _userId;

    /// <summary>
    /// Initiates a new instance of the <see cref="QueryUserReviewsSpecification"/>
    /// class.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    public QueryUserReviewsSpecification(UserId userId)
    {
        _userId = userId;
    }
}
