using Application.Authentication.DTOs.Results;

using MediatR;

namespace Application.Authentication.Queries.LoginCarrier;

/// <summary>
/// Represents a query to log in a carrier.
/// </summary>
/// <param name="Email">The carrier email.</param>
/// <param name="Password">The carrier password.</param>
public record LoginCarrierQuery(
    string Email,
    string Password
) : IRequest<AuthenticationResult>;
