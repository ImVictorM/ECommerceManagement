using Contracts.Authentication;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Abstracts;
using IntegrationTests.Common.Seeds.Carriers;
using IntegrationTests.TestUtils.Constants;
using IntegrationTests.TestUtils.Extensions.Http;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Xunit.Abstractions;
using System.Net.Http.Json;
using System.Net;
using FluentAssertions;

namespace IntegrationTests.Authentication;

/// <summary>
/// Integration tests for the carrier log in feature.
/// </summary>
public class LoginCarrierTests : BaseIntegrationTest
{
    private readonly ICredentialsProvider<CarrierSeedType> _credentialsProvider;

    /// <summary>
    /// Initiates a new instance of the <see cref="LoginCarrierTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public LoginCarrierTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output) : base(factory, output)
    {
        _credentialsProvider = factory.Services.GetRequiredService<ICredentialsProvider<CarrierSeedType>>();
    }

    /// <summary>
    /// Verifies it is possible to login carriers with valid credentials.
    /// </summary>
    /// <param name="carrierType">The carrier type.</param>
    [Theory]
    [InlineData(CarrierSeedType.INTERNAL)]
    public async Task LoginCarrier_WithCorrectCredentials_ReturnsOkContainingToken(CarrierSeedType carrierType)
    {
        var carrierCredentials = _credentialsProvider.GetCredentials(carrierType);
        var request = new LoginCarrierRequest(carrierCredentials.Email, carrierCredentials.Password);
        var endpoint = TestConstants.AuthenticationEndpoints.LoginCarrier;

        var response = await RequestService.Client.PostAsJsonAsync(
            endpoint,
            request
        );

        var authenticationResponse = await response.Content.ReadRequiredFromJsonAsync<AuthenticationResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        authenticationResponse.Should().NotBeNull();
        authenticationResponse.Token.Should().NotBeNullOrWhiteSpace();
        authenticationResponse.Email.Should().Be(request.Email);
    }

    /// <summary>
    /// Verifies a bad request is returned when the email is incorrect.
    /// </summary>
    [Fact]
    public async Task LoginCarrier_WithIncorrectEmail_ReturnsBadRequest()
    {
        var credentials = _credentialsProvider.GetCredentials(CarrierSeedType.INTERNAL);
        var request = new LoginCarrierRequest("incorrect_email@email.com", credentials.Password);
        var endpoint = TestConstants.AuthenticationEndpoints.LoginCarrier;

        var response = await RequestService.Client.PostAsJsonAsync(
            endpoint,
            request
        );
        var authenticationResponse = await response.Content.ReadRequiredFromJsonAsync<ProblemDetails>();

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        authenticationResponse.Should().NotBeNull();
        authenticationResponse.Status.Should().Be((int)HttpStatusCode.BadRequest);
        authenticationResponse.Title.Should().Be("Authentication Failed");
        authenticationResponse.Detail.Should().Be("User email or password is incorrect");
    }

    /// <summary>
    /// Verifies a bad request is returned when the password is incorrect.
    /// </summary>
    [Fact]
    public async Task LoginCarrier_WithIncorrectPassword_ReturnsBadRequest()
    {
        var credentials = _credentialsProvider.GetCredentials(CarrierSeedType.INTERNAL);
        var request = new LoginCarrierRequest(credentials.Email, "IncorrectPassword123");
        var endpoint = TestConstants.AuthenticationEndpoints.LoginCarrier;

        var response = await RequestService.Client.PostAsJsonAsync(
            endpoint,
            request
        );

        var authenticationResponse = await response.Content.ReadRequiredFromJsonAsync<ProblemDetails>();

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        authenticationResponse.Should().NotBeNull();
        authenticationResponse.Status.Should().Be((int)HttpStatusCode.BadRequest);
        authenticationResponse.Title.Should().Be("Authentication Failed");
        authenticationResponse.Detail.Should().Be("User email or password is incorrect");
    }
}
