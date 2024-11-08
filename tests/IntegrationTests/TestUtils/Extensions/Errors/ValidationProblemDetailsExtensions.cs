using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationTests.TestUtils.Extensions.Errors;

/// <summary>
/// Extension methods for the <see cref="ValidationProblemDetails"/> class.
/// </summary>
public static class ValidationProblemDetailsExtensions
{
    /// <summary>
    /// Validates if the validation problem object contains the expected errors.
    /// </summary>
    /// <param name="vpd">The validation problem details object.</param>
    /// <param name="expectedErrors">The expected errors.</param>
    public static void EnsureCorrespondsToExpectedErrors(this ValidationProblemDetails vpd, Dictionary<string, string[]> expectedErrors)
    {
        vpd.Should().NotBeNull();
        vpd!.Status.Should().Be((int)HttpStatusCode.BadRequest);
        vpd.Errors.Should().NotBeEmpty();
        vpd.Errors.Should().ContainKeys(expectedErrors.Keys);

        foreach (var expectedError in expectedErrors)
        {
            var actualMessages = vpd.Errors[expectedError.Key];

            actualMessages.Should().BeEquivalentTo(expectedError.Value);
        }
    }
}
