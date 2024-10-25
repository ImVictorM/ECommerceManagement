using System.Net.Http.Json;
using FluentAssertions;
using IntegrationTests.Authentication.TestUtils;
using IntegrationTests.Common;

namespace IntegrationTests.Authentication;

/// <summary>
/// Integration tests that relate to both register and login processes working together.
/// </summary>
public class RegisterLoginFlowTests : BaseIntegrationTest
{
    /// <summary>
    /// Initiates a new instance of the <see cref="RegisterLoginFlowTests"/> class.
    /// </summary>
    /// <param name="webAppFactory">The test server.</param>
    public RegisterLoginFlowTests(IntegrationTestWebAppFactory webAppFactory) : base(webAppFactory)
    {
    }

    /// <summary>
    /// Tests if it is possible to register then log in the newly created user correctly.
    /// </summary>
    /// <returns>An asynchronous operation.</returns>
    [Fact]
    public async Task RegisterThenLogin_AfterCreatingAnUserAndTryingToAuthenticateWithIt_AuthenticateTheUserCorrectly()
    {
        var registerRequest = RegisterRequestUtils.CreateRequest();
        var loginRequest = LoginRequestUtils.CreateRequest(registerRequest.Email, registerRequest.Password);

        var registerResponse = await Client.PostAsJsonAsync("/auth/register", registerRequest);
        var loginResponse = await Client.PostAsJsonAsync("/auth/login", loginRequest);

        registerResponse.Should().BeSuccessful();
        loginResponse.Should().BeSuccessful();
    }
}
