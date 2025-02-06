using Domain.UserAggregate;

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
}
