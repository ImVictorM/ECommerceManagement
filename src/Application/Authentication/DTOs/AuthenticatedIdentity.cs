namespace Application.Authentication.DTOs;

/// <summary>
/// Represents an authenticated user.
/// </summary>
/// <param name="Id">The user id.</param>
/// <param name="Name">The user name.</param>
/// <param name="Email">The user email.</param>
/// <param name="Phone">The user phone.</param>
public record AuthenticatedIdentity(
    string Id,
    string Name,
    string Email,
    string? Phone
);
