using Contracts.Authentication;

using IntegrationTests.Common.Requests.Abstracts;
using IntegrationTests.Common.Seeds.Carriers;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.TestUtils.Extensions.Http;

using WebApi.Authentication;

using System.Net.Http.Json;
using Microsoft.AspNetCore.Routing;

namespace IntegrationTests.Common.Requests;

/// <summary>
/// Defines a service to handle request operations.
/// </summary>
public sealed class RequestService : IRequestService
{
    private readonly IUserCredentialsProvider _userCredentialsProvider;
    private readonly ICarrierCredentialsProvider _carrierCredentialsProvider;
    private readonly IHttpClientFactory _clientFactory;

    private readonly LinkGenerator _linkGenerator;

    /// <summary>
    /// Initiates a new instance of the <see cref="RequestService"/> class.
    /// </summary>
    /// <param name="clientFactory">
    /// The client factory to create and expose client
    /// .</param>
    /// <param name="userCredentialsProvider">
    /// The user credentials provider.
    /// </param>
    /// <param name="carrierCredentialsProvider">
    /// The carrier credentials provider.
    /// </param>
    /// <param name="linkGenerator">
    /// The link generator.
    /// </param>
    public RequestService(
        IHttpClientFactory clientFactory,
        IUserCredentialsProvider userCredentialsProvider,
        ICarrierCredentialsProvider carrierCredentialsProvider,
        LinkGenerator linkGenerator
    )
    {
        _clientFactory = clientFactory;
        _userCredentialsProvider = userCredentialsProvider;
        _carrierCredentialsProvider = carrierCredentialsProvider;
        _linkGenerator = linkGenerator;
    }

    /// <inheritdoc/>
    public async Task<HttpClient> LoginAsAsync(UserSeedType userType)
    {
        var client = CreateClient();

        var credentials = _userCredentialsProvider.GetCredentials(userType);

        var request = new LoginUserRequest(credentials.Email, credentials.Password);

        var endpoint = _linkGenerator.GetPathByName(
            nameof(AuthenticationEndpoints.LoginUser)
        );

        var response = await client.PostAsJsonAsync(endpoint, request);

        var responseContent = await response.Content
            .ReadRequiredFromJsonAsync<AuthenticationResponse>();

        client.SetJwtBearerAuthorizationHeader(responseContent.Token);

        return client;
    }

    /// <inheritdoc/>
    public async Task<HttpClient> LoginAsAsync(CarrierSeedType carrierType)
    {
        var client = CreateClient();

        var credentials = _carrierCredentialsProvider.GetCredentials(carrierType);

        var request = new LoginCarrierRequest(credentials.Email, credentials.Password);

        var endpoint = _linkGenerator.GetPathByName(
            nameof(AuthenticationEndpoints.LoginCarrier)
        );

        var response = await client.PostAsJsonAsync(endpoint, request);

        var responseContent = await response.Content
            .ReadRequiredFromJsonAsync<AuthenticationResponse>();

        client.SetJwtBearerAuthorizationHeader(responseContent.Token);

        return client;
    }

    /// <inheritdoc/>
    public HttpClient CreateClient()
    {
        return _clientFactory.CreateClient();
    }
}
