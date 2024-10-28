namespace Contracts.Users;

/// <summary>
/// Multiple users response type.
/// </summary>
/// <param name="Users">The users list.</param>
public record UserListResponse(IEnumerable<UserResponse> Users);
