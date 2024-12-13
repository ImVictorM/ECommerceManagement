using MediatR;

namespace Application.Users.Commands.UpdateUser;

/// <summary>
/// Represents a command to update user information.
/// </summary>
/// <param name="IdCurrentUser">The unique identifier of the current user (the one responsible for updating the other user).</param>
/// <param name="IdUserToUpdate">The unique identifier of the user to be updated.</param>
/// <param name="Name">The updated name of the user.</param>
/// <param name="Phone">The updated phone number of the user (Optional).</param>
/// <param name="Email">The updated email address of the user.</param>
public record UpdateUserCommand(
    string IdCurrentUser,
    string IdUserToUpdate,
    string Name,
    string? Phone,
    string Email
) : IRequest<Unit>;
