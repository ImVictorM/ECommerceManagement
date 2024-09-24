using Application.Common.Interfaces.Persistence;
using Domain.UserAggregate;

namespace Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository to interact and persist user data.
/// </summary>
public sealed class UserRepository : IUserRepository
{
    /// <inheritdoc/>
    public Task AddAsync(User user)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public User? GetUserByEmailAddress(string emailAddress)
    {
        throw new NotImplementedException();
    }
}
