using Domain.UserAggregate;

namespace Application.Users.Common;

/// <summary>
/// Result when getting multiple users.
/// </summary>
/// <param name="Users">The user list.</param>
public record UserListResult(IEnumerable<User> Users);
