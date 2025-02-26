using Application.Common.Persistence.Repositories;

using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;

using Infrastructure.Common.Persistence;

namespace Infrastructure.Users;

internal sealed class UserRepository : BaseRepository<User, UserId>, IUserRepository
{
    public UserRepository(IECommerceDbContext dbContext) : base(dbContext)
    {
    }
}
