using Application.Authentication.Common;
using MediatR;

namespace Application.Authentication.Queries.Login;

/// <summary>
/// Command to authenticate a registered user.
/// </summary>
/// <param name="Email">The user email.</param>
/// <param name="Password">The user password.</param>
public record LoginCommand(string Email, string Password): IRequest<AuthenticationResult>;
