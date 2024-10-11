using Domain.UserAggregate.ValueObjects;
using Domain.UserAggregate;

namespace Application.Common.Interfaces.Persistence;

/// <summary>
/// The component used for atomic operation between repositories.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// The user repository.
    /// </summary>
    public IRepository<User, UserId> UserRepository { get; }
    /// <summary>
    /// Save all the operations done within the repositories.
    /// </summary>
    /// <returns>An asynchronous operation containing the number of entries modified.</returns>
    Task<int> SaveChangesAsync();
}
