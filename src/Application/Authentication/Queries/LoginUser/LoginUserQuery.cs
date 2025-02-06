using Application.Authentication.DTOs;

using MediatR;

namespace Application.Authentication.Queries.LoginUser;

/// <summary>
/// Represents a query to authenticate an already registered user.
/// </summary>
/// <param name="Email">The user email.</param>
/// <param name="Password">The user password.</param>
public record LoginUserQuery(string Email, string Password) : IRequest<AuthenticationResult>;
