using Microsoft.AspNetCore.Mvc.Testing;

namespace IntegrationTests.Common.Requests;

/// <summary>
/// Defines a http client factory so that it is possible to use the
/// default pre-configured app factory client with dependency injection.
/// </summary>
/// <typeparam name="TStartup">The startup project.</typeparam>
public class TestHttpClientFactory<TStartup> : IHttpClientFactory
    where TStartup : class
{
    private readonly WebApplicationFactory<TStartup> _appFactory;

    /// <summary>
    /// Initiates a new instance of the
    /// <see cref="TestHttpClientFactory{TStartup}"/> class.
    /// </summary>
    /// <param name="appFactory">The app factory used to create the client.</param>
    public TestHttpClientFactory(WebApplicationFactory<TStartup> appFactory)
    {
        _appFactory = appFactory;
    }

    /// <inheritdoc/>
    public HttpClient CreateClient(string name)
    {
        return _appFactory.CreateClient();
    }
}
