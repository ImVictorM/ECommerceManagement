namespace Contracts.Authentication;

/// <summary>
/// Represents a request to register a new customer user.
/// </summary>
/// <param name="Name">The customer name.</param>
/// <param name="Email">The customer email.</param>
/// <param name="Password">The customer password.</param>
public record RegisterCustomerRequest(
    string Name,
    string Email,
    string Password
);
