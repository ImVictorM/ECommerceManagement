using Application.Common.Interfaces.Persistence;
using Domain.ProductAggregate;
using Domain.ProductAggregate.ValueObjects;
using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;

namespace Infrastructure.Persistence;

/// <summary>
/// Defines the component used for atomic operation between repositories.
/// </summary>
public sealed class UnitOfWork : IUnitOfWork
{
    private readonly ECommerceDbContext _context;

    /// <inheritdoc/>
    public IRepository<User, UserId> UserRepository { get; }

    /// <inheritdoc/>
    public IRepository<Product, ProductId> ProductRepository { get; }

    /// <summary>
    /// Initiates a new instance of the <see cref="UnitOfWork"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    /// <param name="userRepository">The user repository.</param>
    /// <param name="productRepository">The product repository.</param>
    public UnitOfWork(
        ECommerceDbContext context,
        IRepository<User, UserId> userRepository,
        IRepository<Product, ProductId> productRepository
    )
    {
        _context = context;

        UserRepository = userRepository;
        ProductRepository = productRepository;
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
