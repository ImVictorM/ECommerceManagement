using Domain.UserAggregate;

namespace Application.Users.Common.DTOs;

/// <summary>
/// Result when getting a single user.
/// </summary>
/// <param name="User">The user.</param>
public record UserResult(User User);
