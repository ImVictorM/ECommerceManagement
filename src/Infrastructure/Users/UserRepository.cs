using Application.Common.Persistence.Repositories;
using Application.Users.DTOs.Filters;

using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;

using Infrastructure.Common.Persistence;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Users;

internal sealed class UserRepository : BaseRepository<User, UserId>, IUserRepository
{
    public UserRepository(IECommerceDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IReadOnlyList<User>> GetUsersAsync(
        UserFilters filters,
        CancellationToken cancellationToken = default
    )
    {
        var query = DbSet.AsQueryable();

        if (filters.IsActive.HasValue)
        {
            query = query.Where(user => user.IsActive == filters.IsActive);
        }

        return await query
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
