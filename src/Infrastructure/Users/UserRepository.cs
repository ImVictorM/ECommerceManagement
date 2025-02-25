using Application.Common.Persistence.Repositories;

using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;

using Infrastructure.Common.Persistence;

namespace Infrastructure.Users;

/// <summary>
/// Defines the implementation for user persistence operations.
/// </summary>
public sealed class UserRepository : BaseRepository<User, UserId>, IUserRepository
{
    /// <summary>
    /// Initiates a new instance of the <see cref="UserRepository"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public UserRepository(ECommerceDbContext dbContext) : base(dbContext)
    {
    }
}
