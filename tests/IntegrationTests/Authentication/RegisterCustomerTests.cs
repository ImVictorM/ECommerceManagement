using IntegrationTests.Authentication.TestUtils;
using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.TestUtils.Extensions.Authentication;
using IntegrationTests.TestUtils.Extensions.Http;

using Contracts.Authentication;
using RegisterCustomerRequest = Contracts.Authentication.RegisterCustomerRequest;

using WebApi.Authentication;

using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Net;
using FluentAssertions;
using Xunit.Abstractions;

namespace IntegrationTests.Authentication;

/// <summary>
/// Integration tests for the register customer feature.
/// </summary>
public class RegisterCustomerTests : BaseIntegrationTest
{
    private readonly IUserSeed _userSeed;
    private readonly string? _registerEndpoint;
    private readonly string? _loginEndpoint;
    private readonly HttpClient _client;

    /// <summary>
    /// Initiates a new instance of the <see cref="RegisterCustomerTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public RegisterCustomerTests(
        IntegrationTestWebAppFactory factory,
        ITestOutputHelper output
    ) : base(factory, output)
    {
        _userSeed = SeedManager.GetSeed<IUserSeed>();

        _registerEndpoint = LinkGenerator.GetPathByName(
            nameof(AuthenticationEndpoints.RegisterCustomer
        ));

        _loginEndpoint = LinkGenerator.GetPathByName(
            nameof(AuthenticationEndpoints.LoginUser
        ));

        _client = RequestService.CreateClient();
    }

    /// <summary>
    /// Provides a list of valid register request objects with unique emails.
    /// </summary>
    public static readonly IEnumerable<object[]> ValidRequests =
    [
        [
            RegisterCustomerRequestUtils.CreateRequest(
                email: "testing1@email.com"
            )
        ],
        [
            RegisterCustomerRequestUtils.CreateRequest(
                email: "testing2@email.com",
                name: "Testing name"
            )
        ],
        [
            RegisterCustomerRequestUtils.CreateRequest(
                email: "testing3@email.com",
                password: "Super_secret_pass123"
            )
        ],
    ];

    /// <summary>
    /// Verifies it is possible to create customers with valid requests.
    /// Also verifies it is possible to authenticate using the login endpoint
    /// after registration.
    /// </summary>
    /// <param name="registerRequest">The request.</param>
    [Theory]
    [MemberData(nameof(ValidRequests))]
    public async Task RegisterCustomer_WithValidParameters_ReturnsCreatedWithToken(
        RegisterCustomerRequest registerRequest
    )
    {
        var loginRequest = new LoginUserRequest(
            registerRequest.Email,
            registerRequest.Password
        );

        var registerResponse = await _client.PostAsJsonAsync(
            _registerEndpoint,
            registerRequest
        );
        var loginResponse = await _client.PostAsJsonAsync(
            _loginEndpoint,
            loginRequest
        );

        var registerResponseContent = await registerResponse.Content
            .ReadRequiredFromJsonAsync<AuthenticationResponse>();
        var loginResponseContent = await loginResponse.Content
            .ReadRequiredFromJsonAsync<AuthenticationResponse>();

        registerResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        registerResponseContent.EnsureCreatedFromRequest(registerRequest);
        loginResponseContent.EnsureCreatedFromRequest(registerRequest);
    }

    /// <summary>
    /// Verifies creating a user with duplicated email returns a conflict error.
    /// </summary>
    [Fact]
    public async Task RegisterCustomer_WithDuplicatedEmail_ReturnsConflict()
    {
        var existentUser = _userSeed.GetEntity(UserSeedType.CUSTOMER);

        var registerRequest = RegisterCustomerRequestUtils.CreateRequest(
            email: existentUser.Email.ToString()
        );

        var response = await _client.PostAsJsonAsync(
            _registerEndpoint,
            registerRequest
        );

        var responseContent = await response.Content
            .ReadRequiredFromJsonAsync<ProblemDetails>();

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        responseContent.Should().NotBeNull();
        responseContent.Status.Should().Be((int)HttpStatusCode.Conflict);
        responseContent.Title.Should().Be("Email Conflict");
        responseContent.Detail.Should().Be(
            "The email you entered is already in use"
        );
    }
}
