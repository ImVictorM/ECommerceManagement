using Application.Common.Persistence;

namespace Infrastructure.Common.Persistence;

internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly IECommerceDbContext _context;

    public UnitOfWork(IECommerceDbContext context)
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
