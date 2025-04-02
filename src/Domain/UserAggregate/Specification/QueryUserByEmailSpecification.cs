using SharedKernel.Models;
using SharedKernel.ValueObjects;

using System.Linq.Expressions;

namespace Domain.UserAggregate.Specification;

/// <summary>
/// Query specification to fetch a user by email.
/// </summary>
public class QueryUserByEmailSpecification : CompositeQuerySpecification<User>
{
    private readonly Email _email;

    /// <summary>
    /// Initiates a new instance of the
    /// <see cref="QueryUserByEmailSpecification"/> class.
    /// </summary>
    /// <param name="email">The user email.</param>
    public QueryUserByEmailSpecification(Email email)
    {
        _email = email;
    }
    /// <inheritdoc/>
    public override Expression<Func<User, bool>> Criteria => user
        => user.Email == _email;
}
