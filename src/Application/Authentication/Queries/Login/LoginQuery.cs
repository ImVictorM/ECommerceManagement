using Application.Authentication.Common;
using MediatR;

namespace Application.Authentication.Queries.Login;

/// <summary>
/// Query to authenticate a registered user.
/// </summary>
/// <param name="Email">The user email.</param>
/// <param name="Password">The user password.</param>
public record LoginQuery(string Email, string Password): IRequest<AuthenticationResult>;
