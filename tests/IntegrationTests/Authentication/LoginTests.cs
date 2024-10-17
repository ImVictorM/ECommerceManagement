using System.Net.Http.Json;
using Contracts.Authentication;
using FluentAssertions;
using IntegrationTests.Common;
using IntegrationTests.TestUtils.Seeds;

namespace IntegrationTests.Authentication;

/// <summary>
/// Integration tests for the login process.
/// </summary>
public class LoginTests : BaseIntegrationTest
{
    /// <summary>
    /// Initiates a new instance of the <see cref="LoginTests"/> class.
    /// </summary>
    /// <param name="webAppFactory">The test server.</param>
    public LoginTests(IntegrationTestWebAppFactory webAppFactory) : base(webAppFactory)
    {
    }

    [Fact]
    public async Task Login_WhenCredentialsAreValid_AuthenticateTheUserCorrectly()
    {
        var loginRequest = new LoginRequest(UserSeed.User1.Email.Value, UserSeed.UserPassword);

        var response = await HttpClient.PostAsJsonAsync("/auth/login", loginRequest);

        response.Should().BeSuccessful();
    }
}
