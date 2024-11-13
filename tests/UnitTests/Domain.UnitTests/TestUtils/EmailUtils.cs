using Domain.UnitTests.TestUtils.Constants;
using SharedKernel.ValueObjects;

namespace Domain.UnitTests.TestUtils;

/// <summary>
/// Email utilities.
/// </summary>
public static class EmailUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="Email"/> class.
    /// </summary>
    /// <param name="email">The email address.</param>
    /// <returns>A new instance of the <see cref="Email"/> class.</returns>
    public static Email CreateEmail(string? email = null)
    {
        return Email.Create(email ?? DomainConstants.Email.Value);
    }

    /// <summary>
    /// Gets a list of invalid emails with the corresponding errors similar to the validation problem details object.
    /// </summary>
    public static IEnumerable<(string Value, Dictionary<string, string[]>)> GetInvalidEmailsWithCorrespondingErrors()
    {
        yield return (
            "",
            new Dictionary<string, string[]> { { "Email", [
                DomainConstants.Email.Validations.EmptyEmail,
                DomainConstants.Email.Validations.InvalidPatternEmail
            ] } }
        );

        yield return (
            "invalidemailformat",
            new Dictionary<string, string[]> { { "Email", [
                DomainConstants.Email.Validations.InvalidPatternEmail
            ] } }
        );

        yield return (
            "invalidemailformat@invalid@.com",
            new Dictionary<string, string[]> { { "Email", [
                DomainConstants.Email.Validations.InvalidPatternEmail
            ] } }
        );

        yield return (
            "invalidemailformat@invalid.com.",
            new Dictionary<string, string[]> { { "Email", [
                DomainConstants.Email.Validations.InvalidPatternEmail
            ] } }
        );
    }
}
