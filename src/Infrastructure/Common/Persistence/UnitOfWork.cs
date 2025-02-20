using Application.Common.Persistence;

namespace Infrastructure.Common.Persistence;

/// <summary>
/// Defines the component used for atomic operation between repositories.
/// </summary>
public sealed class UnitOfWork : IUnitOfWork
{
    private readonly ECommerceDbContext _context;

    /// <summary>
    /// Initiates a new instance of the <see cref="UnitOfWork"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public UnitOfWork(ECommerceDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc/>
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        _context.Dispose();
    }
}
