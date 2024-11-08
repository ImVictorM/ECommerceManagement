using System.Net;
using System.Net.Http.Json;
using Contracts.Authentication;
using Domain.UnitTests.TestUtils;
using FluentAssertions;
using IntegrationTests.Authentication.TestUtils;
using IntegrationTests.Common;
using IntegrationTests.TestUtils.Extensions.Authentication;
using IntegrationTests.TestUtils.Extensions.Errors;
using Microsoft.AspNetCore.Mvc;
using Xunit.Abstractions;
using RegisterRequest = Contracts.Authentication.RegisterRequest;

namespace IntegrationTests.Authentication;

/// <summary>
/// Integration tests for the register process.
/// </summary>
public class RegisterTests : BaseIntegrationTest
{
    /// <summary>
    /// Initiates a new instance of the <see cref="RegisterTests"/> class.
    /// </summary>
    /// <param name="factory">The test server factory.</param>
    /// <param name="output">The log helper.</param>
    public RegisterTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output) : base(factory, output)
    {
    }

    /// <summary>
    /// List of valid register request objects with unique emails.
    /// </summary>
    public static IEnumerable<object[]> ValidRequests()
    {
        yield return new object[] { RegisterRequestUtils.CreateRequest(email: "testing1@email.com") };
        yield return new object[] { RegisterRequestUtils.CreateRequest(email: "testing2@email.com", name: "Testing name") };
        yield return new object[] { RegisterRequestUtils.CreateRequest(email: "testing3@email.com", password: "Supersecretpass123") };
    }

    /// <summary>
    /// List of invalid register request objects.
    /// </summary>
    public static IEnumerable<object[]> InvalidRequests()
    {
        foreach (var (invalidEmail, expectedErrors) in EmailUtils.GetInvalidEmailsWithCorrespondingErrors())
        {
            yield return new object[] { RegisterRequestUtils.CreateRequest(email: invalidEmail), expectedErrors };
        }

        foreach (var (invalidName, expectedErrors) in UserUtils.GetInvalidNameWithCorrespondingErrors())
        {
            yield return new object[] { RegisterRequestUtils.CreateRequest(name: invalidName), expectedErrors };
        }

        foreach (var (invalidPassword, expectedErrors) in UserUtils.GetInvalidPasswordWithCorrespondingErrors())
        {
            yield return new object[] { RegisterRequestUtils.CreateRequest(password: invalidPassword), expectedErrors };
        }
    }

    /// <summary>
    /// Tests if it is possible to create users with valid requests.
    /// </summary>
    /// <param name="registerRequest">The request object.</param>
    [Theory]
    [MemberData(nameof(ValidRequests))]
    public async Task Register_WhenCreationParametersAreValid_CreatesNewAndReturnWithAuthenticationToken(RegisterRequest registerRequest)
    {
        var loginRequest = LoginRequestUtils.CreateRequest(registerRequest.Email, registerRequest.Password);

        var updateHttpResponse = await Client.PostAsJsonAsync("/auth/register", registerRequest);
        var loginHttpResponse = await Client.PostAsJsonAsync("/auth/login", loginRequest);

        var loginHttpResponseContent = await loginHttpResponse.Content.ReadFromJsonAsync<AuthenticationResponse>();
        var updateHttpResponseContent = await updateHttpResponse.Content.ReadFromJsonAsync<AuthenticationResponse>();

        updateHttpResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        loginHttpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        updateHttpResponseContent!.EnsureCreatedFromRequest(registerRequest);
        loginHttpResponseContent!.EnsureCreatedFromRequest(registerRequest);
    }

    /// <summary>
    /// Tests if it is not possible to create a new user with invalid request parameters.
    /// </summary>
    /// <param name="registerRequest">The request object with invalid parameters.</param>
    /// <param name="expectedErrors">A dictionary containing the field and the expected errors.</param>
    [Theory]
    [MemberData(nameof(InvalidRequests))]
    public async Task Register_WhenCreationParametersAreInvalid_ReturnsBadRequestErrorResponse(
        RegisterRequest registerRequest,
        Dictionary<string, string[]> expectedErrors
    )
    {
        var httpResponse = await Client.PostAsJsonAsync("/auth/register", registerRequest);
        var responseContent = await httpResponse.Content.ReadFromJsonAsync<ValidationProblemDetails>();

        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        responseContent!.EnsureCorrespondsToExpectedErrors(expectedErrors);
    }

    /// <summary>
    /// Tests if creating a duplicated user returns a conflict error.
    /// </summary>
    [Fact]
    public async Task Register_WhenEmailIsAlreadyRegistered_ReturnsConflictErrorResponse()
    {
        var registerRequest = RegisterRequestUtils.CreateRequest();

        await Client.PostAsJsonAsync("/auth/register", registerRequest);

        var httpResponse = await Client.PostAsJsonAsync("/auth/register", registerRequest);
        var responseContent = await httpResponse.Content.ReadFromJsonAsync<ProblemDetails>();

        httpResponse.StatusCode.Should().Be(HttpStatusCode.Conflict);
        responseContent.Should().NotBeNull();
        responseContent!.Status.Should().Be((int)HttpStatusCode.Conflict);
        responseContent.Title.Should().Be("User Conflict");
        responseContent.Detail.Should().Be("The user already exists");
    }
}
