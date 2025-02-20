namespace Application.Common.Persistence;

/// <summary>
/// Defines a unit of work that manages atomic operations across multiple repositories.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Commits all pending changes within the current transaction.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation, returning the number of affected entries.
    /// </returns>
    Task<int> SaveChangesAsync();
}
