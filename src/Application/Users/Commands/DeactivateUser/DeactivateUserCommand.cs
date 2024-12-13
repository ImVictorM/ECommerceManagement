using MediatR;

namespace Application.Users.Commands.DeactivateUser;

/// <summary>
/// Represents a command to deactivate and user.
/// </summary>
/// <param name="IdCurrentUser">The id of the current user responsible for the deactivation of the other user.</param>
/// <param name="IdUserToDeactivate">The id of the user to be deactivated.</param>
public record DeactivateUserCommand(string IdCurrentUser, string IdUserToDeactivate) : IRequest<Unit>;
