namespace IntegrationTests.Common;

/// <summary>
/// Base component of integrations tests.
/// </summary>
public class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>
{
    /// <summary>
    /// The client used to make requests.
    /// </summary>
    protected HttpClient HttpClient { get; init; }

    /// <summary>
    /// Initiates a new instance of the <see cref="BaseIntegrationTest"/> class.
    /// </summary>
    /// <param name="webAppFactory">The test server factory.</param>
    public BaseIntegrationTest(IntegrationTestWebAppFactory webAppFactory)
    {
        HttpClient = webAppFactory.CreateClient();
    }
}
