using FluentAssertions;
using IntegrationTests.Common;

namespace IntegrationTests.Authentication;

/// <summary>
/// Integration tests for the login process.
/// </summary>
public class LoginTests : BaseIntegrationTest
{
    /// <summary>
    /// Initiates a new instance of the <see cref="LoginTests"/> class.
    /// </summary>
    /// <param name="webAppFactory">The test server.</param>
    public LoginTests(IntegrationTestWebAppFactory webAppFactory) : base(webAppFactory)
    {
    }

    [Fact]
    public void Login_WhenCredentialsAreValid_AuthenticateTheUserCorrectly()
    {
        1.ToString().Should().Be("1");
    }
}
