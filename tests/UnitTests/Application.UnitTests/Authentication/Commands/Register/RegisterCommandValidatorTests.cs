using Application.Authentication.Commands.Register;
using Application.UnitTests.Authentication.Commands.TestUtils;
using Application.UnitTests.TestUtils.ValidationData;

using FluentAssertions;
using FluentValidation.TestHelper;

namespace Application.UnitTests.Authentication.Commands.Register;

/// <summary>
/// Unit tests for the <see cref="RegisterCommandValidator"/> validator.
/// </summary>
public class RegisterCommandValidatorTests
{
    private readonly RegisterCommandValidator _validator;

    /// <summary>
    /// Initiates a new instance of the <see cref="RegisterCommandValidatorTests"/> class.
    /// </summary>
    public RegisterCommandValidatorTests()
    {
        _validator = new RegisterCommandValidator();
    }

    /// <summary>
    /// Pairs of invalid password and expected error messages.
    /// </summary>
    public static IEnumerable<object[]> InvalidPasswords =>
    [
        [
            "",
            new List<string>()
            {
                "'Password' must not be empty.",
                "'Password' must be at least 6 characters long.",
                "'Password' must contain at least one digit.",
                "'Password' must contain at least one character.",
            }
        ],
        [
            "123456",
            new List<string>()
            {
                "'Password' must contain at least one character.",
            }
        ],
        [
            "abcdef",
            new List<string>()
            {
                "'Password' must contain at least one digit.",
            }
        ],
        [
            "a2345",
            new List<string>()
            {
                "'Password' must be at least 6 characters long.",
            }
        ]
    ];

    /// <summary>
    /// Tests when the name is invalid the name should have validation errors.
    /// </summary>
    /// <param name="name">The invalid name.</param>
    /// <param name="expectedErrorMessages">The expected error messages.</param>
    [Theory]
    [MemberData(nameof(ValidationTestData.Name.InvalidNames), MemberType = typeof(ValidationTestData.Name))]
    public void ValidateRegisterCommand_WhenNameIsInvalid_ShouldHaveValidationError(
        string name,
        IEnumerable<string> expectedErrorMessages
    )
    {
        var command = RegisterCommandUtils.CreateCommand(name: name);

        var result = _validator.TestValidate(command);

        result.IsValid.Should().BeFalse();

        foreach (var error in expectedErrorMessages)
        {
            result.ShouldHaveValidationErrorFor(c => c.Name).WithErrorMessage(error);
        }
    }

    /// <summary>
    /// Tests when the email is invalid the email should have validation errors.
    /// </summary>
    /// <param name="email">The invalid email.</param>
    /// <param name="expectedErrorMessages">The expected error messages.</param>
    [Theory]
    [MemberData(nameof(ValidationTestData.Email.InvalidEmails), MemberType = typeof(ValidationTestData.Email))]
    public void ValidateRegisterCommand_WhenEmailIsInvalid_ShouldHaveValidationError(
        string email,
        IEnumerable<string> expectedErrorMessages
    )
    {
        var command = RegisterCommandUtils.CreateCommand(email: email);

        var result = _validator.TestValidate(command);

        result.IsValid.Should().BeFalse();

        foreach (var error in expectedErrorMessages)
        {
            result.ShouldHaveValidationErrorFor(c => c.Email).WithErrorMessage(error);
        }
    }

    /// <summary>
    /// Tests when the password is invalid the password should have validation errors.
    /// </summary>
    /// <param name="password">The invalid password.</param>
    /// <param name="expectedErrorMessages">The expected error messages.</param>
    [Theory]
    [MemberData(nameof(InvalidPasswords))]
    public void ValidateRegisterCommand_WhenPasswordIsInvalid_ShouldHaveValidationError(
        string password,
        IEnumerable<string> expectedErrorMessages
    )
    {
        var command = RegisterCommandUtils.CreateCommand(password: password);

        var result = _validator.TestValidate(command);

        result.IsValid.Should().BeFalse();

        foreach (var error in expectedErrorMessages)
        {
            result.ShouldHaveValidationErrorFor(c => c.Password).WithErrorMessage(error);
        }
    }
}
