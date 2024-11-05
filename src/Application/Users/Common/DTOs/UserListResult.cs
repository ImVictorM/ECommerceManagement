using Domain.UserAggregate;

namespace Application.Users.Common.DTOs;

/// <summary>
/// Result when getting multiple users.
/// </summary>
/// <param name="Users">The user list.</param>
public record UserListResult(IEnumerable<User> Users);
