using Domain.UserAggregate.ValueObjects;
using SharedKernel.Models;

namespace Domain.UserAggregate.Specification;

/// <summary>
/// Specification to check if certain user has the privileges to read an order.
/// </summary>
public class ReadOrderSpecification : CompositeSpecification<User>
{
    private readonly UserId _orderOwnerId;

    /// <summary>
    /// Initiates a new instance of the <see cref="ReadOrderSpecification"/> class.
    /// </summary>
    /// <param name="orderOwnerId">The order owner id.</param>
    public ReadOrderSpecification(UserId orderOwnerId)
    {
        _orderOwnerId = orderOwnerId;
    }

    /// <inheritdoc/>
    public override bool IsSatisfiedBy(User currentUser)
    {
        return _orderOwnerId == currentUser.Id || currentUser.IsAdmin();
    }
}
