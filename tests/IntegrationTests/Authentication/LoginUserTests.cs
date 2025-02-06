using IntegrationTests.Common;

using Contracts.Authentication;

using IntegrationTests.Common.Seeds.Abstracts;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.TestUtils.Extensions.Http;

using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests.Authentication;

/// <summary>
/// Integration tests for the user log in feature.
/// </summary>
public class LoginUserTests : BaseIntegrationTest
{
    private readonly ICredentialsProvider<UserSeedType> _credentialsProvider;

    /// <summary>
    /// Initiates a new instance of the <see cref="LoginUserTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public LoginUserTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output) : base(factory, output)
    {
        _credentialsProvider = factory.Services.GetRequiredService<ICredentialsProvider<UserSeedType>>();
    }

    /// <summary>
    /// Checks if it is possible to login active users with valid credentials.
    /// </summary>
    /// <param name="userActiveType">An active user type.</param>
    [Theory]
    [InlineData(UserSeedType.CUSTOMER)]
    [InlineData(UserSeedType.CUSTOMER_WITH_ADDRESS)]
    [InlineData(UserSeedType.ADMIN)]
    [InlineData(UserSeedType.OTHER_ADMIN)]
    public async Task LoginUser_WhenCredentialsAreValidAndUserIsActive_AuthenticateTheUserCorrectly(UserSeedType userActiveType)
    {
        var activeUserCredentials = _credentialsProvider.GetCredentials(userActiveType);
        var request = new LoginUserRequest(activeUserCredentials.Email, activeUserCredentials.Password);

        var httpResponse = await RequestService.Client.PostAsJsonAsync("/auth/login", request);
        var authenticationResponse = await httpResponse.Content.ReadRequiredFromJsonAsync<AuthenticationResponse>();

        httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        authenticationResponse.Should().NotBeNull();
        authenticationResponse.Token.Should().NotBeNullOrWhiteSpace();
        authenticationResponse.Email.Should().Be(request.Email);
    }

    /// <summary>
    /// Checks if it is not possible to login inactive users with valid credentials.
    /// </summary>
    /// <param name="userInactiveType">An inactive user type.</param>
    [Theory]
    [InlineData(UserSeedType.CUSTOMER_INACTIVE)]
    public async Task LoginUser_WhenCredentialsAreValidAndUserIsInactive_ReturnsBadRequest(UserSeedType userInactiveType)
    {
        var inactiveUserCredentials = _credentialsProvider.GetCredentials(userInactiveType);
        var request = new LoginUserRequest(inactiveUserCredentials.Email, inactiveUserCredentials.Password);

        var httpResponse = await RequestService.Client.PostAsJsonAsync("/auth/login", request);
        var authenticationResponse = await httpResponse.Content.ReadRequiredFromJsonAsync<ProblemDetails>();

        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        authenticationResponse.Should().NotBeNull();
        authenticationResponse.Status.Should().Be((int)HttpStatusCode.BadRequest);
        authenticationResponse.Title.Should().Be("Authentication Failed");
        authenticationResponse.Detail.Should().Be("User email or password is incorrect");
    }

    /// <summary>
    /// Verifies it returns a bad request when email is incorrect.
    /// </summary>
    [Fact]
    public async Task LoginUser_WhenEmailIsIncorrect_ReturnsBadRequest()
    {
        var credentials = _credentialsProvider.GetCredentials(UserSeedType.CUSTOMER);
        var request = new LoginUserRequest("incorrect_email@email.com", credentials.Password);

        var httpResponse = await RequestService.Client.PostAsJsonAsync("/auth/login", request);
        var authenticationResponse = await httpResponse.Content.ReadRequiredFromJsonAsync<ProblemDetails>();

        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        authenticationResponse.Should().NotBeNull();
        authenticationResponse.Status.Should().Be((int)HttpStatusCode.BadRequest);
        authenticationResponse.Title.Should().Be("Authentication Failed");
        authenticationResponse.Detail.Should().Be("User email or password is incorrect");
    }

    /// <summary>
    /// Verifies it returns a bad request when password is incorrect.
    /// </summary>
    [Fact]
    public async Task LoginUser_WhenPasswordIsIncorrect_ReturnsBadRequest()
    {
        var credentials = _credentialsProvider.GetCredentials(UserSeedType.CUSTOMER);
        var request = new LoginUserRequest(credentials.Email, "IncorrectPassword123");

        var httpResponse = await RequestService.Client.PostAsJsonAsync("/auth/login", request);
        var authenticationResponse = await httpResponse.Content.ReadRequiredFromJsonAsync<ProblemDetails>();

        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        authenticationResponse.Should().NotBeNull();
        authenticationResponse.Status.Should().Be((int)HttpStatusCode.BadRequest);
        authenticationResponse.Title.Should().Be("Authentication Failed");
        authenticationResponse.Detail.Should().Be("User email or password is incorrect");
    }
}
