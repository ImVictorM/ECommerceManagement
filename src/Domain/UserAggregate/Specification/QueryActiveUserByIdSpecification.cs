using Domain.UserAggregate.ValueObjects;

using SharedKernel.Specifications;

namespace Domain.UserAggregate.Specification;

/// <summary>
/// Query to retrieve an active user by identifier.
/// </summary>
public class QueryActiveUserByIdSpecification
    : QueryActiveEntityByIdSpecification<User, UserId>
{
    /// <summary>
    /// Initiates a new instance of the
    /// <see cref="QueryActiveUserByIdSpecification"/> class.
    /// </summary>
    /// <param name="id">The user identifier.</param>
    public QueryActiveUserByIdSpecification(UserId id) : base(id)
    {
    }
}
