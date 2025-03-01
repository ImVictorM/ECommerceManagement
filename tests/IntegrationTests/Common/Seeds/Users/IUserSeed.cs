using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;

using IntegrationTests.Common.Seeds.Abstracts;

namespace IntegrationTests.Common.Seeds.Users;

/// <summary>
/// Defines a contract to provide seed data for users in the database.
/// </summary>
public interface IUserSeed : IDataSeed<UserSeedType, User, UserId>
{
}
