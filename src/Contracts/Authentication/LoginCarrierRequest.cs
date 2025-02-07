namespace Contracts.Authentication;

/// <summary>
/// Represents a request to log in a carrier.
/// </summary>
/// <param name="Email">The carrier email.</param>
/// <param name="Password">The carrier password.</param>
public record LoginCarrierRequest(
    string Email,
    string Password
);
