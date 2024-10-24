using Domain.UserAggregate;

namespace Application.Users.Queries.GetUserById;

/// <summary>
/// Return type when getting a user by id.
/// </summary>
/// <param name="User">The user.</param>
public record GetUserByIdResult(User User);
