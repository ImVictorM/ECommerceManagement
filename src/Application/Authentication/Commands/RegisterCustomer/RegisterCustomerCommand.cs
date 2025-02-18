using Application.Authentication.DTOs;

using MediatR;

namespace Application.Authentication.Commands.RegisterCustomer;

/// <summary>
/// Command to register a new customer user.
/// </summary>
/// <param name="Name">The customer name.</param>
/// <param name="Email">The customer email.</param>
/// <param name="Password">The customer password.</param>
public record RegisterCustomerCommand(
    string Name,
    string Email,
    string Password
) : IRequest<AuthenticationResult>;
