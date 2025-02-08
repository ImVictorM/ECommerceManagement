using Domain.CarrierAggregate;
using Domain.UserAggregate;

using IntegrationTests.Common.Seeds.Carriers;
using IntegrationTests.Common.Seeds.Users;

namespace IntegrationTests.Common.Requests.Abstracts;

/// <summary>
/// Defines a service to handle request operations.
/// </summary>
public interface IRequestService
{
    /// <summary>
    /// Gets the http client to make requests.
    /// </summary>
    HttpClient Client { get; }
    /// <summary>
    /// Authenticates as a seed user.
    /// </summary>
    /// <param name="userType">The seed user type.</param>
    /// <returns>The authenticated user.</returns>
    Task<User> LoginAsAsync(UserSeedType userType);
    /// <summary>
    /// Authenticates as a seed carrier.
    /// </summary>
    /// <param name="carrierType">The seed carrier type.</param>
    /// <returns>The authenticated carrier.</returns>
    Task<Carrier> LoginAsAsync(CarrierSeedType carrierType);
}
