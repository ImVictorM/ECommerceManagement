using IntegrationTests.Common.Seeds.Carriers;
using IntegrationTests.Common.Seeds.Users;

namespace IntegrationTests.Common.Requests.Abstracts;

/// <summary>
/// Defines a service to handle HTTP request operations.
/// </summary>
public interface IRequestService
{
    /// <summary>
    /// Creates a new instance of an <see cref="HttpClient"/>.
    /// </summary>
    HttpClient CreateClient();

    /// <summary>
    /// Authenticates as a seed user and returns an authenticated HTTP client.
    /// </summary>
    /// <param name="userType">The seed user type</param>
    /// <returns>An authenticated HTTP client</returns>
    Task<HttpClient> LoginAsAsync(UserSeedType userType);

    /// <summary>
    /// Authenticates as a seed carrier and returns an authenticated HTTP client.
    /// </summary>
    /// <param name="carrierType">The seed carrier type</param>
    /// <returns>An authenticated HTTP client</returns>
    Task<HttpClient> LoginAsAsync(CarrierSeedType carrierType);
}
