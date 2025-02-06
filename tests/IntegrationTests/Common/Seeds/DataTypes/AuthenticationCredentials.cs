namespace IntegrationTests.Common.Seeds.DataTypes;

/// <summary>
/// Represents authentication credentials data.
/// </summary>
/// <param name="Email">The authentication email.</param>
/// <param name="Password">The authentication password.</param>
public record AuthenticationCredentials(string Email, string Password);
