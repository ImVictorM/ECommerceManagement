using Domain.UserAggregate.ValueObjects;

using SharedKernel.Models;

using System.Linq.Expressions;

namespace Domain.ProductFeedbackAggregate.Specifications;

/// <summary>
/// Represents a query to retrieve user product feedback.
/// </summary>
public class QueryUserProductFeedback : CompositeQuerySpecification<ProductFeedback>
{
    private readonly UserId _userId;

    /// <inheritdoc/>
    public override Expression<Func<ProductFeedback, bool>> Criteria =>
        feedback => feedback.UserId == _userId;

    /// <summary>
    /// Initiates a new instance of the <see cref="QueryUserProductFeedback"/> class.
    /// </summary>
    /// <param name="userId">The user id.</param>
    public QueryUserProductFeedback(UserId userId)
    {
        _userId = userId;
    }
}
