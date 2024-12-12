using SharedKernel.Models;
using SharedKernel.ValueObjects;

namespace Domain.UserAggregate.Specification;

/// <summary>
/// Query specification to fetch an user by email.
/// </summary>
public class QueryUserByEmailSpecification : CompositeQuerySpecification<User>
{
    /// <summary>
    /// Initiates a new instance of the <see cref="QueryUserByEmailSpecification"/> class.
    /// </summary>
    /// <param name="email">The user email.</param>
    public QueryUserByEmailSpecification(Email email) : base(user => user.Email == email)
    {
    }
}
