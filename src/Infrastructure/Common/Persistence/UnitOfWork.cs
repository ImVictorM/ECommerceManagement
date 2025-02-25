using Application.Common.Persistence;

namespace Infrastructure.Common.Persistence;

internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly ECommerceDbContext _context;

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
