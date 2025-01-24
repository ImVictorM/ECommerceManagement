using Application.Authentication.Common.DTOs;

using MediatR;

namespace Application.Authentication.Queries.Login;

/// <summary>
/// Represents a query to authenticate an already registered user.
/// </summary>
/// <param name="Email">The user email.</param>
/// <param name="Password">The user password.</param>
public record LoginQuery(string Email, string Password) : IRequest<AuthenticationResult>;
