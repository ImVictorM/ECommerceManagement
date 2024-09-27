using Application.Common.Interfaces.Persistence;
using Domain.RoleAggregate;
using Domain.RoleAggregate.ValueObjects;
using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;

namespace Infrastructure.Persistence;

/// <summary>
/// Defines the component used for atomic operation between repositories.
/// </summary>
public sealed class UnitOfWork : IUnitOfWork
{
    /// <summary>
    /// The database context.
    /// </summary>
    private readonly ECommerceDbContext _context;

    /// <inheritdoc/>
    public IRepository<User, UserId> UserRepository { get; }
    /// <inheritdoc/>
    public IRepository<Role, RoleId> RoleRepository { get; }

    /// <summary>
    /// Initiates a new instance of the <see cref="UnitOfWork"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    /// <param name="userRepository">The user repository.</param>
    /// <param name="roleRepository">The role repository.</param>
    public UnitOfWork(
        ECommerceDbContext context,
        IRepository<User, UserId> userRepository,
        IRepository<Role, RoleId> roleRepository
    )
    {
        _context = context;
        UserRepository = userRepository;
        RoleRepository = roleRepository;
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
