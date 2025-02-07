using Contracts.Authentication;

using Domain.UserAggregate;

using IntegrationTests.Common.Requests.Abstracts;
using IntegrationTests.Common.Seeds.Abstracts;
using IntegrationTests.Common.Seeds.Users;
using IntegrationTests.TestUtils.Constants;
using IntegrationTests.TestUtils.Extensions.Http;

using System.Net.Http.Json;

namespace IntegrationTests.Common.Requests;

/// <summary>
/// Defines a service to handle request operations.
/// </summary>
public sealed class RequestService : IRequestService
{
    private readonly ICredentialsProvider<UserSeedType> _credentialsProvider;
    private readonly IDataSeed<UserSeedType, User> _userSeed;

    /// <inheritdoc/>
    public HttpClient Client { get; }

    /// <summary>
    /// Initiates a new instance of the <see cref="RequestService"/> class.
    /// </summary>
    /// <param name="clientFactory">The client factory to create and expose client.</param>
    /// <param name="credentialsProvider">The user credentials provider.</param>
    /// <param name="seedManager">The seed manager.</param>
    public RequestService(
        IHttpClientFactory clientFactory,
        ICredentialsProvider<UserSeedType> credentialsProvider,
        ISeedManager seedManager
    )
    {
        Client = clientFactory.CreateClient();
        _credentialsProvider = credentialsProvider;
        _userSeed = seedManager.GetSeed<UserSeedType, User>();
    }

    /// <inheritdoc/>
    public async Task<User> LoginAsAsync(UserSeedType userType)
    {
        var credentials = _credentialsProvider.GetCredentials(userType);

        var request = new LoginUserRequest(credentials.Email, credentials.Password);

        var response = await Client.PostAsJsonAsync(TestConstants.AuthenticationEndpoints.LoginUser, request);

        var responseContent = await response.Content.ReadRequiredFromJsonAsync<AuthenticationResponse>();

        Client.SetJwtBearerAuthorizationHeader(responseContent.Token);

        return _userSeed.GetByType(userType);
    }
}
