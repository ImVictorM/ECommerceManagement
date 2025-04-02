using Application.Users.DTOs.Filters;

using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;

namespace Application.Common.Persistence.Repositories;

/// <summary>
/// Defines the contract for user persistence operations.
/// </summary>
public interface IUserRepository : IBaseRepository<User, UserId>
{
    /// <summary>
    /// Retrieves the users matching the filtering criteria.
    /// </summary>
    /// <param name="filters">
    /// The filters to apply when querying the users.
    /// </param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A list of <see cref="User"/>.</returns>
    Task<IReadOnlyList<User>> GetUsersAsync(
        UserFilters filters,
        CancellationToken cancellationToken = default
    );
}
