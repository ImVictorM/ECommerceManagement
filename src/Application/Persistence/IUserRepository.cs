using Domain.Users;

namespace Application.Persistence;

/// <summary>
/// Repository to interact and persist user data.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Gets an user by the given email.
    /// </summary>
    /// <param name="emailAddress">The user email.</param>
    /// <returns>The user find by email or null</returns>
    User? GetUserByEmailAddress(string emailAddress);

    /// <summary>
    /// Add a new user asynchronously.
    /// </summary>
    /// <param name="user">The user to be persisted.</param>
    /// <returns>A task indicating the operation.</returns>
    Task AddAsync(User user);
}
