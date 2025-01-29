using Domain.UserAggregate;

namespace Application.Users.DTOs;

/// <summary>
/// Result when getting a single user.
/// </summary>
/// <param name="User">The user.</param>
public record UserResult(User User);
