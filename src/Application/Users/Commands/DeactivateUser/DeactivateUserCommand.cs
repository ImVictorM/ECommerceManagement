using MediatR;

namespace Application.Users.Commands.DeactivateUser;

/// <summary>
/// Represents a command to deactivate and user.
/// </summary>
/// <param name="Id">The user to be deactivated identifier.</param>
public record DeactivateUserCommand(string Id) : IRequest<Unit>;
