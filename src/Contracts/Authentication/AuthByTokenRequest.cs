namespace Contracts.Authentication;

/// <summary>
/// Request type to authenticate an user using their authentication token.
/// </summary>
/// <param name="Token">The user token.</param>
public record AuthByTokenRequest(string Token);
