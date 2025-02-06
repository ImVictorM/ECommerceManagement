using Contracts.Authentication;
using RegisterCustomerRequest = Contracts.Authentication.RegisterCustomerRequest;

using FluentAssertions;

namespace IntegrationTests.TestUtils.Extensions.Authentication;

/// <summary>
/// Defines extension methods for the <see cref="AuthenticationResponse"/> class.
/// </summary>
public static class AuthenticationResponseExtensions
{
    /// <summary>
    /// Checks if the authentication response has the same information of the request and other fields are initiated correctly.
    /// </summary>
    /// <param name="response">The response object.</param>
    /// <param name="request">The request object.</param>
    public static void EnsureCreatedFromRequest(this AuthenticationResponse response, RegisterCustomerRequest request)
    {
        response.Should().NotBeNull();
        response!.Token.Should().NotBeNullOrWhiteSpace();
        response.Name.Should().Be(request.Name);
        response.Email.Should().Be(request.Email);
        response.Id.Should().NotBeNullOrWhiteSpace();
        response.Phone.Should().BeNull();
    }
}
