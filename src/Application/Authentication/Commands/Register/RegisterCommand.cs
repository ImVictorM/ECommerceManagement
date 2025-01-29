using Application.Authentication.DTOs;

using MediatR;

namespace Application.Authentication.Commands.Register;

/// <summary>
/// Command to register a new user.
/// </summary>
/// <param name="Name">The user name.</param>
/// <param name="Email">The user email.</param>
/// <param name="Password">The user password.</param>
public record RegisterCommand(
    string Name,
    string Email,
    string Password
) : IRequest<AuthenticationResult>;
