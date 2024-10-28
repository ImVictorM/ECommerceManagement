using Domain.UserAggregate;

namespace Application.Users.Common;

/// <summary>
/// Result when getting a single user.
/// </summary>
/// <param name="User">The user.</param>
public record UserResult(User User);
