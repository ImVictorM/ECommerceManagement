using System.Net;
using System.Net.Http.Json;
using Contracts.Authentication;
using FluentAssertions;
using IntegrationTests.Common;
using IntegrationTests.TestUtils.Seeds;
using Microsoft.AspNetCore.Http;

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

    /// <summary>
    /// List of valid login request objects.
    /// </summary>
    /// <returns>A list of valid login requests.</returns>
    public static IEnumerable<object[]> ValidRequests()
    {
        yield return new object[] { new LoginRequest(UserSeed.User1.Email.Value, UserSeed.UserPassword) };
        yield return new object[] { new LoginRequest(UserSeed.User2.Email.Value, UserSeed.UserPassword) };
        yield return new object[] { new LoginRequest(UserSeed.Admin.Email.Value, UserSeed.AdminPassword) };
    }

    /// <summary>
    /// List of invalid login request objects.
    /// </summary>
    /// <returns>A list of invalid login requests.</returns>
    public static IEnumerable<object[]> InvalidRequests()
    {
 

        yield return new object[] { new LoginRequest("", ""), "Email", "Password" };
        yield return new object[] { new LoginRequest("          ", "        "), "Email", "Password" };
        yield return new object[] { new LoginRequest(UserSeed.User1.Email.Value, ""), "Password" };
        yield return new object[] { new LoginRequest("", UserSeed.UserPassword), "Email" };
    }

    /// <summary>
    /// Checks if it is possible to login with valid credentials.
    /// </summary>
    /// <param name="loginRequest">The request object.</param>
    /// <returns>An asynchronous operation.</returns>
    [Theory]
    [MemberData(nameof(ValidRequests))]
    public async Task Login_WhenCredentialsAreValid_AuthenticateTheUserCorrectly(LoginRequest loginRequest)
    {
        var httpResponse = await HttpClient.PostAsJsonAsync("/auth/login", loginRequest);
        var authenticationResponse = await httpResponse.Content.ReadFromJsonAsync<AuthenticationResponse>();

        httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        authenticationResponse.Should().NotBeNull();
        authenticationResponse!.Token.Should().NotBeNullOrWhiteSpace();
        authenticationResponse!.Email.Should().Be(loginRequest.Email);
    }

    /// <summary>
    /// Checks if it returs a bad request response when the login credentials are invalid.
    /// </summary>
    /// <param name="loginRequest">The invalid request object.</param>
    /// <param name="fieldsWithError">The fields that will contain some error.</param>
    /// <returns>An asynchronous operation.</returns>
    [Theory]
    [MemberData(nameof(InvalidRequests))]
    public async Task Login_WhenCredentialAreInvalid_ReturnsCorrectErrorResponse(
        LoginRequest loginRequest,
        params string[] fieldsWithError
    )
    {
        var httpResponse = await HttpClient.PostAsJsonAsync("/auth/login", loginRequest);

        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errorResponse = await httpResponse.Content.ReadFromJsonAsync<HttpValidationProblemDetails>();

        errorResponse.Should().NotBeNull();
        errorResponse!.Status.Should().Be((int)HttpStatusCode.BadRequest);
        errorResponse.Errors.Count.Should().Be(fieldsWithError.Length);

        foreach (var field in fieldsWithError)
        {
            errorResponse.Errors[field].Should().NotBeEmpty();
            errorResponse.Errors[field][0].Should().MatchRegex("^* must not be empty");
        }
    }
}
