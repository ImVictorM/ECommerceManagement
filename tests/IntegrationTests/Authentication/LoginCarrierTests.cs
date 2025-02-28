using Contracts.Authentication;

using IntegrationTests.Common;
using IntegrationTests.Common.Seeds.Abstracts;
using IntegrationTests.Common.Seeds.Carriers;
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
/// Integration tests for the carrier login feature.
/// </summary>
public class LoginCarrierTests : BaseIntegrationTest
{
    private readonly ICredentialsProvider<CarrierSeedType> _credentialsProvider;
    private readonly string? _endpoint;
    private readonly HttpClient _client;

    /// <summary>
    /// Initiates a new instance of the <see cref="LoginCarrierTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public LoginCarrierTests(
        IntegrationTestWebAppFactory factory,
        ITestOutputHelper output
    ) : base(factory, output)
    {
        _credentialsProvider = factory.Services
            .GetRequiredService<ICredentialsProvider<CarrierSeedType>>();

        _client = RequestService.CreateClient();

        _endpoint = LinkGenerator.GetPathByName(
            nameof(AuthenticationEndpoints.LoginCarrier
        ));
    }

    /// <summary>
    /// Verifies it is possible to login carriers with correct credentials.
    /// </summary>
    /// <param name="carrierType">The carrier type.</param>
    [Theory]
    [InlineData(CarrierSeedType.INTERNAL)]
    public async Task LoginCarrier_WithCorrectCredentials_ReturnsOk(
        CarrierSeedType carrierType
    )
    {
        var carrierCredentials = _credentialsProvider.GetCredentials(carrierType);
        var request = new LoginCarrierRequest(
            carrierCredentials.Email,
            carrierCredentials.Password
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
    /// Verifies a bad request is returned when the email is incorrect.
    /// </summary>
    [Fact]
    public async Task LoginCarrier_WithIncorrectEmail_ReturnsBadRequest()
    {
        var credentials = _credentialsProvider.GetCredentials(
            CarrierSeedType.INTERNAL
        );

        var request = new LoginCarrierRequest(
            "incorrect_email@email.com",
            credentials.Password
        );

        var response = await _client.PostAsJsonAsync(
            _endpoint,
            request
        );

        var authenticationResponse = await response.Content
            .ReadRequiredFromJsonAsync<ProblemDetails>();

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
        var request = new LoginCarrierRequest(
            credentials.Email,
            "IncorrectPassword123"
        );

        var response = await _client.PostAsJsonAsync(
            _endpoint,
            request
        );

        var authenticationResponse = await response.Content
            .ReadRequiredFromJsonAsync<ProblemDetails>();

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        authenticationResponse.Should().NotBeNull();
        authenticationResponse.Status.Should().Be((int)HttpStatusCode.BadRequest);
        authenticationResponse.Title.Should().Be("Authentication Failed");
        authenticationResponse.Detail.Should().Be("User email or password is incorrect");
    }
}
