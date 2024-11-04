using System.Net;
using System.Net.Http.Json;
using Contracts.Authentication;
using FluentAssertions;
using IntegrationTests.Authentication.TestUtils;
using IntegrationTests.Common;
using IntegrationTests.TestUtils.Seeds;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    /// List of login request containing credentials for active users.
    /// </summary>
    /// <returns>A list of valid login requests.</returns>
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
    /// <returns>A list of invalid login requests.</returns>
    public static IEnumerable<object[]> InactiveUserRequests()
    {
        foreach (var (Email, Password) in UserSeed.ListUsersCredentials(user => !user.IsActive))
        {
            yield return new object[] { LoginRequestUtils.CreateRequest(Email, Password) };
        }
    }

    /// <summary>
    /// List of invalid login request objects.
    /// </summary>
    /// <returns>A list of invalid login requests.</returns>
    public static IEnumerable<object[]> RequestWithInvalidParameters()
    {


        yield return new object[] { LoginRequestUtils.CreateRequest("", ""), "Email", "Password" };
        yield return new object[] { LoginRequestUtils.CreateRequest("          ", "        "), "Email", "Password" };
        yield return new object[] { LoginRequestUtils.CreateRequest(password: ""), "Password" };
        yield return new object[] { LoginRequestUtils.CreateRequest(email: ""), "Email" };
    }

    /// <summary>
    /// List of login requests with incorrect pairs of email and password.
    /// </summary>
    /// <returns>List of incorrect email and password pairs.</returns>
    public static IEnumerable<object[]> IncorrectEmailPasswordPairs()
    {
        yield return new object[] { LoginRequestUtils.CreateRequest(password: "incorrectpassword") };
        yield return new object[] { LoginRequestUtils.CreateRequest(email: "incorrect@email.com") };
    }

    /// <summary>
    /// Checks if it is possible to login active users with valid credentials.
    /// </summary>
    /// <param name="loginRequest">The request object.</param>
    /// <returns>An asynchronous operation.</returns>
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
    /// <returns>An asynchronous operation.</returns>
    [Theory]
    [MemberData(nameof(InactiveUserRequests))]
    public async Task Login_WhenCredentialsAreValidAndUserIsInactive_ReturnsBadRequest(LoginRequest loginRequest)
    {
        var httpResponse = await Client.PostAsJsonAsync("/auth/login", loginRequest);
        var authenticationResponse = await httpResponse.Content.ReadFromJsonAsync<ProblemDetails>();

        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        authenticationResponse!.Should().NotBeNull();
        authenticationResponse!.Status.Should().Be((int)HttpStatusCode.BadRequest);
        authenticationResponse!.Title.Should().Be("Authentication Failed.");
        authenticationResponse!.Detail.Should().Be("User email or password is incorrect.");
    }

    /// <summary>
    /// Checks if it returns a bad request response when the login credentials are invalid.
    /// </summary>
    /// <param name="loginRequest">The invalid request object.</param>
    /// <param name="fieldsWithError">The fields that will contain some error.</param>
    /// <returns>An asynchronous operation.</returns>
    [Theory]
    [MemberData(nameof(RequestWithInvalidParameters))]
    public async Task Login_WhenCredentialAreInvalid_ReturnsCorrectErrorResponse(
        LoginRequest loginRequest,
        params string[] fieldsWithError
    )
    {
        var httpResponse = await Client.PostAsJsonAsync("/auth/login", loginRequest);

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

    /// <summary>
    /// Tests if it returns a bad request with the same message when the authentication credentials are incorrect.
    /// </summary>
    /// <param name="loginRequest">The request object containing invalid email/password pairs.</param>
    /// <returns>An asynchronous operation.</returns>
    [Theory]
    [MemberData(nameof(IncorrectEmailPasswordPairs))]
    public async Task Login_WhenEmailOrPasswordIsIncorrect_ReturnsBadRequest(LoginRequest loginRequest)
    {
        var httpResponse = await Client.PostAsJsonAsync("/auth/login", loginRequest);
        var authenticationResponse = await httpResponse.Content.ReadFromJsonAsync<ProblemDetails>();

        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        authenticationResponse.Should().NotBeNull();
        authenticationResponse!.Status.Should().Be((int)HttpStatusCode.BadRequest);
        authenticationResponse!.Title.Should().Be("Authentication Failed.");
        authenticationResponse!.Detail.Should().Be("User email or password is incorrect.");
    }
}
