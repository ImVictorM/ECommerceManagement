using Domain.UserAggregate;

using IntegrationTests.Authentication.TestUtils;
using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.TestUtils.Extensions.Authentication;
using IntegrationTests.Common.Seeds.Abstracts;
using IntegrationTests.TestUtils.Extensions.Http;
using IntegrationTests.TestUtils.Constants;

using Contracts.Authentication;
using RegisterCustomerRequest = Contracts.Authentication.RegisterCustomerRequest;

using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit.Abstractions;

namespace IntegrationTests.Authentication;

/// <summary>
/// Integration tests for the register customer feature.
/// </summary>
public class RegisterCustomerTests : BaseIntegrationTest
{
    
    private readonly IDataSeed<UserSeedType, User> _userSeed;

    /// <summary>
    /// Initiates a new instance of the <see cref="RegisterCustomerTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public RegisterCustomerTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output) : base(factory, output)
    {
        _userSeed = SeedManager.GetSeed<UserSeedType, User>();
    }

    /// <summary>
    /// List of valid register request objects with unique emails.
    /// </summary>
    public static readonly IEnumerable<object[]> ValidRequests =
    [
        [RegisterCustomerRequestUtils.CreateRequest(email: "testing1@email.com")],
        [RegisterCustomerRequestUtils.CreateRequest(email: "testing2@email.com", name: "Testing name")],
        [RegisterCustomerRequestUtils.CreateRequest(email: "testing3@email.com", password: "Super_secret_pass123")],
    ];

    /// <summary>
    /// Tests if it is possible to create users with valid requests.
    /// Also tests it is possible to authenticate using the login endpoint after register.
    /// </summary>
    /// <param name="registerRequest">The request object.</param>
    [Theory]
    [MemberData(nameof(ValidRequests))]
    public async Task RegisterCustomer_WithValidParameters_CreatesNewUserAndAuthenticateThem(RegisterCustomerRequest registerRequest)
    {
        var loginRequest = new LoginUserRequest(registerRequest.Email, registerRequest.Password);

        var registerHttpResponse = await RequestService.Client.PostAsJsonAsync(
            TestConstants.AuthenticationEndpoints.RegisterCustomer,
            registerRequest
        );
        var loginHttpResponse = await RequestService.Client.PostAsJsonAsync(
            TestConstants.AuthenticationEndpoints.LoginUser,
            loginRequest
        );

        var loginHttpResponseContent = await loginHttpResponse.Content.ReadRequiredFromJsonAsync<AuthenticationResponse>();
        var registerHttpResponseContent = await registerHttpResponse.Content.ReadRequiredFromJsonAsync<AuthenticationResponse>();

        registerHttpResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        loginHttpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        registerHttpResponseContent.EnsureCreatedFromRequest(registerRequest);
        loginHttpResponseContent.EnsureCreatedFromRequest(registerRequest);
    }

    /// <summary>
    /// Tests creating a user with duplicated email returns a conflict error.
    /// </summary>
    [Fact]
    public async Task RegisterCustomer_WithDuplicatedEmail_ReturnsConflictErrorResponse()
    {
        var existingUser = _userSeed.GetByType(UserSeedType.CUSTOMER);

        var registerRequest = RegisterCustomerRequestUtils.CreateRequest(email: existingUser.Email.ToString());

        await RequestService.Client.PostAsJsonAsync(
            TestConstants.AuthenticationEndpoints.RegisterCustomer,
            registerRequest
        );

        var httpResponse = await RequestService.Client.PostAsJsonAsync(
            TestConstants.AuthenticationEndpoints.RegisterCustomer,
            registerRequest
        );

        var responseContent = await httpResponse.Content.ReadRequiredFromJsonAsync<ProblemDetails>();

        httpResponse.StatusCode.Should().Be(HttpStatusCode.Conflict);
        responseContent.Should().NotBeNull();
        responseContent.Status.Should().Be((int)HttpStatusCode.Conflict);
        responseContent.Title.Should().Be("Email Conflict");
        responseContent.Detail.Should().Be("The email you entered is already in use");
    }
}
