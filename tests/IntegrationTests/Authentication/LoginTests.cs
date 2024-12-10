using System.Net;
using System.Net.Http.Json;
using Contracts.Authentication;
using FluentAssertions;
using IntegrationTests.Authentication.TestUtils;
using IntegrationTests.Common;
using IntegrationTests.TestUtils.Seeds;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit.Abstractions;

namespace IntegrationTests.Authentication;

/// <summary>
/// Integration tests for the login process.
/// </summary>
public class LoginTests : BaseIntegrationTest
{
    /// <summary>
    /// Initiates a new instance of the <see cref="LoginTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public LoginTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output) : base(factory, output)
    {
    }

    /// <summary>
    /// List of login request containing credentials for active users.
    /// </summary>
    public static IEnumerable<object[]> ActiveUserRequests()
    {
        foreach (var (Email, Password) in UserSeed.ListUsersCredentials(user => user.IsActive))
        {
            yield return new object[] { LoginRequestUtils.CreateRequest(Email, Password) };
        }
    }

    /// <summary>
    /// List of login request containing credentials for inactive users.
    /// </summary>
    public static IEnumerable<object[]> InactiveUserRequests()
    {
        foreach (var (Email, Password) in UserSeed.ListUsersCredentials(user => !user.IsActive))
        {
            yield return new object[] { LoginRequestUtils.CreateRequest(Email, Password) };
        }
    }

    /// <summary>
    /// List of login requests with incorrect pairs of email and password.
    /// </summary>
    public static IEnumerable<object[]> RequestsWithWrongCredentials()
    {
        yield return new object[] { LoginRequestUtils.CreateRequest(password: "incorrectpassword") };
        yield return new object[] { LoginRequestUtils.CreateRequest(email: "incorrect@email.com") };
    }

    /// <summary>
    /// Checks if it is possible to login active users with valid credentials.
    /// </summary>
    /// <param name="loginRequest">The request object.</param>
    [Theory]
    [MemberData(nameof(ActiveUserRequests))]
    public async Task Login_WhenCredentialsAreValidAndUserIsActive_AuthenticateTheUserCorrectly(LoginRequest loginRequest)
    {
        var httpResponse = await Client.PostAsJsonAsync("/auth/login", loginRequest);
        var authenticationResponse = await httpResponse.Content.ReadFromJsonAsync<AuthenticationResponse>();

        httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        authenticationResponse.Should().NotBeNull();
        authenticationResponse!.Token.Should().NotBeNullOrWhiteSpace();
        authenticationResponse!.Email.Should().Be(loginRequest.Email);
    }

    /// <summary>
    /// Checks if it is not possible to login inactive users with valid credentials.
    /// </summary>
    /// <param name="loginRequest">The request object.</param>
    [Theory]
    [MemberData(nameof(InactiveUserRequests))]
    public async Task Login_WhenCredentialsAreValidAndUserIsInactive_ReturnsBadRequest(LoginRequest loginRequest)
    {
        var httpResponse = await Client.PostAsJsonAsync("/auth/login", loginRequest);
        var authenticationResponse = await httpResponse.Content.ReadFromJsonAsync<ProblemDetails>();

        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        authenticationResponse!.Should().NotBeNull();
        authenticationResponse!.Status.Should().Be((int)HttpStatusCode.BadRequest);
        authenticationResponse!.Title.Should().Be("Authentication Failed");
        authenticationResponse!.Detail.Should().Be("User email or password is incorrect");
    }

    /// <summary>
    /// Tests if it returns a bad request with the same message when the authentication credentials are incorrect.
    /// </summary>
    /// <param name="loginRequest">The request object containing invalid email/password pairs.</param>
    [Theory]
    [MemberData(nameof(RequestsWithWrongCredentials))]
    public async Task Login_WhenEmailOrPasswordIsIncorrect_ReturnsBadRequest(LoginRequest loginRequest)
    {
        var httpResponse = await Client.PostAsJsonAsync("/auth/login", loginRequest);
        var authenticationResponse = await httpResponse.Content.ReadFromJsonAsync<ProblemDetails>();

        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        authenticationResponse.Should().NotBeNull();
        authenticationResponse!.Status.Should().Be((int)HttpStatusCode.BadRequest);
        authenticationResponse!.Title.Should().Be("Authentication Failed");
        authenticationResponse!.Detail.Should().Be("User email or password is incorrect");
    }
}
