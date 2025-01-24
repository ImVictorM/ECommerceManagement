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
    public static readonly IEnumerable<object[]> ValidEmails =
    [
        ["valid@email.com"],
        ["valid_email@email.com"],
        ["valid@email"],
        ["123@$.xyz"],
        ["\"valid yet strange\"@email.com"],
    ];

    /// <summary>
    /// List of considered invalid emails.
    /// </summary>
    public static readonly IEnumerable<object[]> InvalidEmails =
    [
        ["invalid"],
        ["some_invalid_Email.com"],
        ["email@invalid.com."],
        ["email@email@invalid.com"],
    ];

    /// <summary>
    /// Tests if it is possible to create email value objects with valid email addresses.
    /// </summary>
    /// <param name="emailAddress">The email address.</param>
    [Theory]
    [MemberData(nameof(ValidEmails))]
    public void CreateEmail_WithValidEmailAddress_CreatesNewInstance(string emailAddress)
    {
        var actionResult = FluentActions
            .Invoking(() => EmailUtils.CreateEmail(emailAddress))
            .Should()
            .NotThrow();

        var email = actionResult.Subject;

        email.Should().NotBeNull();
        email.Value.Should().Be(emailAddress);
    }

    /// <summary>
    /// Test if it throws an error when trying to create an email instance with an invalid email address.
    /// </summary>
    /// <param name="emailAddress">The email address.</param>
    [Theory]
    [MemberData(nameof(InvalidEmails))]
    public void CreateEmail_WithInvalidEmailAddress_ThrowsError(string emailAddress)
    {
        FluentActions
            .Invoking(() => EmailUtils.CreateEmail(emailAddress))
            .Should()
            .Throw<InvalidPatternException>()
            .WithMessage($"The {emailAddress} does not correspond to a valid email");
    }
}
