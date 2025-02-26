using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;

namespace Application.Common.Persistence.Repositories;

/// <summary>
/// Defines the contract for user persistence operations.
/// </summary>
public interface IUserRepository : IBaseRepository<User, UserId>
{
}
