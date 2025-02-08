using Domain.CarrierAggregate;
using Domain.UserAggregate;

using Contracts.Authentication;

using IntegrationTests.Common.Requests.Abstracts;
using IntegrationTests.Common.Seeds.Abstracts;
using IntegrationTests.Common.Seeds.Carriers;
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
    private readonly ICredentialsProvider<UserSeedType> _userCredentialsProvider;
    private readonly ICredentialsProvider<CarrierSeedType> _carrierCredentialsProvider;

    private readonly IDataSeed<UserSeedType, User> _seedUser;
    private readonly IDataSeed<CarrierSeedType, Carrier> _seedCarrier;

    /// <inheritdoc/>
    public HttpClient Client { get; }

    /// <summary>
    /// Initiates a new instance of the <see cref="RequestService"/> class.
    /// </summary>
    /// <param name="clientFactory">The client factory to create and expose client.</param>
    /// <param name="userCredentialsProvider">The user credentials provider.</param>
    /// <param name="carrierCredentialsProvider">The carrier credentials provider.</param>
    /// <param name="seedManager">The seed manager.</param>
    public RequestService(
        IHttpClientFactory clientFactory,
        ICredentialsProvider<UserSeedType> userCredentialsProvider,
        ICredentialsProvider<CarrierSeedType> carrierCredentialsProvider,
        ISeedManager seedManager
    )
    {
        Client = clientFactory.CreateClient();
        _userCredentialsProvider = userCredentialsProvider;
        _carrierCredentialsProvider = carrierCredentialsProvider;
        _seedUser = seedManager.GetSeed<UserSeedType, User>();
        _seedCarrier = seedManager.GetSeed<CarrierSeedType, Carrier>();
    }

    /// <inheritdoc/>
    public async Task<User> LoginAsAsync(UserSeedType userType)
    {
        var credentials = _userCredentialsProvider.GetCredentials(userType);

        var request = new LoginUserRequest(credentials.Email, credentials.Password);

        var response = await Client.PostAsJsonAsync(TestConstants.AuthenticationEndpoints.LoginUser, request);

        var responseContent = await response.Content.ReadRequiredFromJsonAsync<AuthenticationResponse>();

        Client.SetJwtBearerAuthorizationHeader(responseContent.Token);

        return _seedUser.GetByType(userType);
    }

    /// <inheritdoc/>
    public async Task<Carrier> LoginAsAsync(CarrierSeedType carrierType)
    {
        var credentials = _carrierCredentialsProvider.GetCredentials(carrierType);

        var request = new LoginCarrierRequest(credentials.Email, credentials.Password);

        var response = await Client.PostAsJsonAsync(TestConstants.AuthenticationEndpoints.LoginCarrier, request);

        var responseContent = await response.Content.ReadRequiredFromJsonAsync<AuthenticationResponse>();

        Client.SetJwtBearerAuthorizationHeader(responseContent.Token);

        return _seedCarrier.GetByType(carrierType);
    }
}
