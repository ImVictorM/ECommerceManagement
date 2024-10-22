using System.Net;
using System.Net.Http.Json;
using Contracts.Authentication;
using FluentAssertions;
using IntegrationTests.Authentication.TestUtils;
using IntegrationTests.Common;
using Microsoft.AspNetCore.Mvc;
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
    /// <param name="webAppFactory">The test server.</param>
    public RegisterTests(IntegrationTestWebAppFactory webAppFactory) : base(webAppFactory)
    {
    }

    /// <summary>
    /// List of valid register request objects with unique emails.
    /// </summary>
    /// <returns>A list of valid request objects.</returns>
    public static IEnumerable<object[]> ValidRequests()
    {
        yield return new object[] { RegisterRequestUtils.CreateRequest(email: "testing1@email.com") };
        yield return new object[] { RegisterRequestUtils.CreateRequest(email: "testing2@email.com", name: "Testing name") };
        yield return new object[] { RegisterRequestUtils.CreateRequest(email: "testing3@email.com", password: "Supersecretpass123") };
    }

    /// <summary>
    /// List of invalid register request objects.
    /// </summary>
    /// <returns>A list of invalid register request objects.</returns>
    public static IEnumerable<object[]> InvalidRequests()
    {
        const string emailInvalidPatternMessage = "'Email' does not follow the required pattern.";

        yield return new object[] {
            RegisterRequestUtils.CreateRequest(email: ""),
            new Dictionary<string, string[]>()
            {
                { "Email", ["'Email' must not be empty.", emailInvalidPatternMessage] }
            }
        };
        yield return new object[] {
            RegisterRequestUtils.CreateRequest(email: "invalidemailformat"),
            new Dictionary<string, string[]>()
            {
                { "Email", [emailInvalidPatternMessage] }
            }
        };
        yield return new object[] {
            RegisterRequestUtils.CreateRequest(email: "invalidemailformat@invalid@.com"),
            new Dictionary<string, string[]>()
            {
                { "Email", [emailInvalidPatternMessage] }
            }
        };
        yield return new object[] {
            RegisterRequestUtils.CreateRequest(email: "invalidemailformat@invalid.com."),
            new Dictionary<string, string[]>()
            {
                { "Email", [emailInvalidPatternMessage] }
            }
        };
        yield return new object[] {
            RegisterRequestUtils.CreateRequest(name: ""),
            new Dictionary<string, string[]>()
            {
                { "Name", ["'Name' must not be empty.", "'Name' must be at least 3 characters long."] }
            }
        };
        yield return new object[] {
            RegisterRequestUtils.CreateRequest(name: "7S"),
            new Dictionary<string, string[]>()
            {
                { "Name", ["'Name' must be at least 3 characters long."] }
            }
        };
        yield return new object[] {
            RegisterRequestUtils.CreateRequest(password: ""),
            new Dictionary<string, string[]>()
            {
                {
                    "Password", [
                        "'Password' must not be empty.",
                        "'Password' must be at least 6 characters long.",
                        "'Password' must contain at least one digit.",
                        "'Password' must contain at least one character."
                    ]
                }
            }
        };
        yield return new object[] {
            RegisterRequestUtils.CreateRequest(password: "123456"),
            new Dictionary<string, string[]>()
            {
                { "Password", ["'Password' must contain at least one character."] }
            }
        };
        yield return new object[] {
            RegisterRequestUtils.CreateRequest(password: "a2345"),
            new Dictionary<string, string[]>()
            {
                { "Password", ["'Password' must be at least 6 characters long."] }
            }
        };
    }

    /// <summary>
    /// Tests if it is possible to create users with valid requests.
    /// </summary>
    /// <param name="registerRequest">The request object.</param>
    /// <returns>An asynchronous operation.</returns>
    [Theory]
    [MemberData(nameof(ValidRequests))]
    public async Task Register_WhenCreationParametersAreValid_CreatesNewAndReturnWithAuthenticationToken(RegisterRequest registerRequest)
    {
        var httpResponse = await HttpClient.PostAsJsonAsync("/auth/register", registerRequest);

        var responseContent = await httpResponse.Content.ReadFromJsonAsync<AuthenticationResponse>();

        httpResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        responseContent.Should().NotBeNull();
        responseContent!.Token.Should().NotBeNullOrWhiteSpace();
        responseContent.Email.Should().Be(registerRequest.Email);
        responseContent.Id.Should().BePositive();
        responseContent.Phone.Should().BeNull();
    }

    /// <summary>
    /// Tests if it is not possible to create a new user with invalid request parameters.
    /// </summary>
    /// <param name="registerRequest">The request object with invalid parameters.</param>
    /// <param name="expectedErrors">A dictionary containing the field and the expected errors.</param>
    /// <returns>An asynchronous operation.</returns>
    [Theory]
    [MemberData(nameof(InvalidRequests))]
    public async Task Register_WhenCreationParametersAreInvalid_RetunsBadRequestErrorResponse(
        RegisterRequest registerRequest,
        Dictionary<string, string[]> expectedErrors
    )
    {
        var httpResponse = await HttpClient.PostAsJsonAsync("/auth/register", registerRequest);
        var responseContent = await httpResponse.Content.ReadFromJsonAsync<ValidationProblemDetails>();

        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        responseContent.Should().NotBeNull();
        responseContent!.Status.Should().Be((int)HttpStatusCode.BadRequest);
        responseContent.Errors.Should().NotBeEmpty();
        responseContent.Errors.Should().ContainKeys(expectedErrors.Keys);

        foreach (var expectedError in expectedErrors)
        {
            var actualMessages = responseContent.Errors[expectedError.Key];

            actualMessages.Should().BeEquivalentTo(expectedError.Value);
        }
    }

    /// <summary>
    /// Tests if creating a duplicated user returns a conflict error.
    /// </summary>
    /// <returns>An asynchronous operation.</returns>
    [Fact]
    public async Task Register_WhenEmailIsAlreadyRegistered_ReturnsConflictErrorResponse()
    {
        var registerRequest = RegisterRequestUtils.CreateRequest();

        await HttpClient.PostAsJsonAsync("/auth/register", registerRequest);

        var httpResponse = await HttpClient.PostAsJsonAsync("/auth/register", registerRequest);
        var responseContent = await httpResponse.Content.ReadFromJsonAsync<ProblemDetails>();

        httpResponse.StatusCode.Should().Be(HttpStatusCode.Conflict);
        responseContent.Should().NotBeNull();
        responseContent!.Status.Should().Be((int)HttpStatusCode.Conflict);
        responseContent.Title.Should().Be("There was an error when trying to create a new user.");
        responseContent.Detail.Should().Be("User already exists.");
    }
}
