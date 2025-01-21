using Domain.UserAggregate.ValueObjects;

using SharedKernel.Models;

using System.Linq.Expressions;

namespace Domain.OrderAggregate.Specifications;

/// <summary>
/// Specification to query a customer order.
/// </summary>
public sealed class QueryCustomerOrderSpecification : CompositeQuerySpecification<Order>
{
    /// <inheritdoc/>
    public override Expression<Func<Order, bool>> Criteria { get; }

    /// <summary>
    /// Initiates a new instance of the <see cref="QueryCustomerOrderSpecification"/> class.
    /// </summary>
    /// <param name="ownerId">The order owner id.</param>
    public QueryCustomerOrderSpecification(UserId ownerId)
    {
        Criteria = order => order.OwnerId == ownerId;
    }
}
