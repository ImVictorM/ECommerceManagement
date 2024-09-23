using Application.Common.Interfaces.Persistence;
using Domain.UserAggregate;

namespace Infrastructure.Persistence.Repositories;

public sealed class UserRepository : IUserRepository
{
    public Task AddAsync(User user)
    {
        throw new NotImplementedException();
    }

    public User? GetUserByEmailAddress(string emailAddress)
    {
        throw new NotImplementedException();
    }
}
