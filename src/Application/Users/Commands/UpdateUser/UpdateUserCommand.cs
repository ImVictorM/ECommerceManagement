using MediatR;

namespace Application.Users.Commands.UpdateUser;

/// <summary>
/// Represents a command to update user information.
/// </summary>
/// <param name="Id">The unique identifier of the user to be updated.</param>
/// <param name="Name">The updated name of the user.</param>
/// <param name="Phone">The updated phone number of the user (Optional).</param>
/// <param name="Email">The updated email address of the user.</param>
public record UpdateUserCommand(
    string Id,
    string Name,
    string? Phone,
    string Email
) : IRequest;
