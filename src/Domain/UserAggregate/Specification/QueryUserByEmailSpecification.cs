using System.Linq.Expressions;
using SharedKernel.Models;
using SharedKernel.ValueObjects;

namespace Domain.UserAggregate.Specification;

/// <summary>
/// Query specification to fetch an user by email.
/// </summary>
public class QueryUserByEmailSpecification : CompositeQuerySpecification<User>
{
    private readonly Email _email;

    /// <summary>
    /// Initiates a new instance of the <see cref="QueryUserByEmailSpecification"/> class.
    /// </summary>
    /// <param name="email">The user email.</param>
    public QueryUserByEmailSpecification(Email email)
    {
        _email = email;
    }
    /// <inheritdoc/>
    public override Expression<Func<User, bool>> Criteria => user => user.Email == _email;
}
