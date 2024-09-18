using Application.Common.Interfaces.Persistence;
using Domain.Users;

namespace Infrastructure.Persistence;

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
