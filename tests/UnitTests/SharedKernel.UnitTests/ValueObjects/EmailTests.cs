using FluentAssertions;
using SharedKernel.Errors;
using SharedKernel.UnitTests.TestUtils;

namespace SharedKernel.UnitTests.ValueObjects;

/// <summary>
/// Tests for the <see cref="SharedKernel.ValueObjects.Email"/> value object.
/// </summary>
public class EmailTests
{
    /// <summary>
    /// List of considered valid emails.
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<object[]> ValidEmails()
    {
        yield return new[] { "valid@email.com" };
        yield return new[] { "valid_email@email.com" };
        yield return new[] { "valid@email" };
        yield return new[] { "123@$.xyz" };
        yield return new[] { "\"valid yet strange\"@email.com" };
    }

    /// <summary>
    /// List of considered invalid emails.
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<object[]> InvalidEmails()
    {
        yield return new[] { "invalid" };
        yield return new[] { "someinvalidEmail.com" };
        yield return new[] { "email@invalid.com." };
        yield return new[] { "email@email@invalid.com" };
    }

    /// <summary>
    /// Tests if it is possible to create email value objects with valid email addresses.
    /// </summary>
    /// <param name="emailAddress">The email address.</param>
    [Theory]
    [MemberData(nameof(ValidEmails))]
    public void Email_WhenEmailAddressIsValid_CreatesNewInstance(string emailAddress)
    {
        var email = EmailUtils.CreateEmail(emailAddress);

        email.Should().NotBeNull();
        email.Value.Should().Be(emailAddress);
    }

    /// <summary>
    /// Test if it throws an error when trying to create an email instance with an invalid email address.
    /// </summary>
    /// <param name="emailAddress">The email address.</param>
    [Theory]
    [MemberData(nameof(InvalidEmails))]
    public void Email_WhenEmailAddressIsNotValid_GeneratesAnError(string emailAddress)
    {
        Action act = () => EmailUtils.CreateEmail(emailAddress);

        act.Should().Throw<DomainValidationException>().WithMessage($"The {emailAddress} does not correspond to a valid email");
    }
}
