using Domain.OrderAggregate.Enumerations;

using SharedKernel.Models;

using System.Linq.Expressions;

namespace Domain.OrderAggregate.Specifications;

/// <summary>
/// Represents a query specification to filter orders by status.
/// If status is null, the filter is not applied.
/// </summary>
public sealed class QueryOrderByStatusSpecification : CompositeQuerySpecification<Order>
{
    /// <inheritdoc/>
    public override Expression<Func<Order, bool>> Criteria { get; }

    /// <summary>
    /// Initiates a new instance of the <see cref="QueryOrderByStatusSpecification"/> class.
    /// </summary>
    /// <param name="status">The status filter.</param>
    public QueryOrderByStatusSpecification(OrderStatus? status = null)
    {
        Criteria = status is not null
            ? order => order.OrderStatusId == status.Id
            : _ => true;
    }
}
