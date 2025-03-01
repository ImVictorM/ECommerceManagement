using IntegrationTests.Common;

using Contracts.Authentication;

using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.TestUtils.Extensions.Http;

using WebApi.Authentication;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Net;
using Xunit.Abstractions;
using FluentAssertions;

namespace IntegrationTests.Authentication;

/// <summary>
/// Integration tests for the user login feature.
/// </summary>
public class LoginUserTests : BaseIntegrationTest
{
    private readonly IUserCredentialsProvider _credentialsProvider;
    private readonly string? _endpoint;
    private readonly HttpClient _client;

    /// <summary>
    /// Initiates a new instance of the <see cref="LoginUserTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public LoginUserTests(
        IntegrationTestWebAppFactory factory,
        ITestOutputHelper output
    ) : base(factory, output)
    {
        _credentialsProvider = factory.Services
            .GetRequiredService<IUserCredentialsProvider>();

        _endpoint = LinkGenerator.GetPathByName(
            nameof(AuthenticationEndpoints.LoginUser
        ));

        _client = RequestService.CreateClient();
    }

    /// <summary>
    /// Verifies it is possible to login active users with correct credentials.
    /// </summary>
    /// <param name="userActiveType">The active user type.</param>
    [Theory]
    [InlineData(UserSeedType.CUSTOMER)]
    [InlineData(UserSeedType.CUSTOMER_WITH_ADDRESS)]
    [InlineData(UserSeedType.ADMIN)]
    [InlineData(UserSeedType.OTHER_ADMIN)]
    public async Task LoginUser_WithCorrectActiveUserCredentials_ReturnsOk(
        UserSeedType userActiveType
    )
    {
        var activeUserCredentials = _credentialsProvider.GetCredentials(
            userActiveType
        );

        var request = new LoginUserRequest(
            activeUserCredentials.Email,
            activeUserCredentials.Password
        );

        var response = await _client.PostAsJsonAsync(
            _endpoint,
            request
        );

        var responseContent = await response.Content
            .ReadRequiredFromJsonAsync<AuthenticationResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseContent.Should().NotBeNull();
        responseContent.Token.Should().NotBeNullOrWhiteSpace();
        responseContent.Email.Should().Be(request.Email);
    }

    /// <summary>
    /// Verifies it is not possible to login inactive users with correct credentials.
    /// </summary>
    /// <param name="userInactiveType">The inactive user type.</param>
    [Theory]
    [InlineData(UserSeedType.CUSTOMER_INACTIVE)]
    public async Task LoginUser_WithCorrectInactiveUserCredentials_ReturnsBadRequest(
        UserSeedType userInactiveType
    )
    {
        var inactiveUserCredentials = _credentialsProvider.GetCredentials(
            userInactiveType
        );

        var request = new LoginUserRequest(
            inactiveUserCredentials.Email,
            inactiveUserCredentials.Password
        );

        var response = await _client.PostAsJsonAsync(
            _endpoint,
            request
        );

        var responseContent = await response.Content
            .ReadRequiredFromJsonAsync<ProblemDetails>();

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        responseContent.Should().NotBeNull();
        responseContent.Status.Should().Be((int)HttpStatusCode.BadRequest);
        responseContent.Title.Should().Be("Authentication Failed");
        responseContent.Detail.Should().Be("User email or password is incorrect");
    }

    /// <summary>
    /// Verifies a bad request is returned when the email is incorrect.
    /// </summary>
    [Fact]
    public async Task LoginUser_WithIncorrectEmail_ReturnsBadRequest()
    {
        var credentials = _credentialsProvider.GetCredentials(
            UserSeedType.CUSTOMER
        );

        var request = new LoginUserRequest(
            "incorrect_email@email.com",
            credentials.Password
        );

        var response = await _client.PostAsJsonAsync(
            _endpoint,
            request
        );

        var responseContent = await response.Content
            .ReadRequiredFromJsonAsync<ProblemDetails>();

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        responseContent.Should().NotBeNull();
        responseContent.Status.Should().Be((int)HttpStatusCode.BadRequest);
        responseContent.Title.Should().Be("Authentication Failed");
        responseContent.Detail.Should().Be("User email or password is incorrect");
    }

    /// <summary>
    /// Verifies a bad request is returned when the password is incorrect.
    /// </summary>
    [Fact]
    public async Task LoginUser_WithIncorrectPassword_ReturnsBadRequest()
    {
        var credentials = _credentialsProvider.GetCredentials(
            UserSeedType.CUSTOMER
        );
        var request = new LoginUserRequest(
            credentials.Email,
            "IncorrectPassword123"
        );

        var response = await _client.PostAsJsonAsync(
            _endpoint,
            request
        );

        var responseContent = await response.Content
            .ReadRequiredFromJsonAsync<ProblemDetails>();

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        responseContent.Should().NotBeNull();
        responseContent.Status.Should().Be((int)HttpStatusCode.BadRequest);
        responseContent.Title.Should().Be("Authentication Failed");
        responseContent.Detail.Should().Be("User email or password is incorrect");
    }
}
