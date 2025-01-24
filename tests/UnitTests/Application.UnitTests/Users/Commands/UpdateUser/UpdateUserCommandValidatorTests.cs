using Application.UnitTests.TestUtils.ValidationData;
using Application.UnitTests.Users.Commands.TestUtils;
using Application.Users.Commands.UpdateUser;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace Application.UnitTests.Users.Commands.UpdateUser;

/// <summary>
/// Unit tests for the <see cref="UpdateUserCommandValidator"/> validator.
/// </summary>
public class UpdateUserCommandValidatorTests
{
    private readonly UpdateUserCommandValidator _validator;

    /// <summary>
    /// Initiates a new instance of the <see cref="UpdateUserCommandValidatorTests"/> class.
    /// </summary>
    public UpdateUserCommandValidatorTests()
    {
        _validator = new UpdateUserCommandValidator();
    }

    /// <summary>
    /// Tests when the command name is invalid the name should have validation errors.
    /// </summary>
    /// <param name="name">The invalid name.</param>
    /// <param name="expectedErrorMessages">The expected error messages.</param>
    [Theory]
    [MemberData(nameof(ValidationTestData.Name.InvalidNames), MemberType = typeof(ValidationTestData.Name))]
    public void ValidateUpdateUserCommand_WhenNameIsInvalid_ShouldHaveValidationError(
        string name,
        IEnumerable<string> expectedErrorMessages
    )
    {
        var command = UpdateUserCommandUtils.CreateCommand(name: name);

        var result = _validator.TestValidate(command);

        result.IsValid.Should().BeFalse();

        foreach (var expectedErrorMessage in expectedErrorMessages)
        {
            result.ShouldHaveValidationErrorFor(c => c.Name).WithErrorMessage(expectedErrorMessage);
        }
    }

    /// <summary>
    /// Tests when the command email is invalid the email should have validation errors.
    /// </summary>
    /// <param name="email">The invalid email.</param>
    /// <param name="expectedErrorMessages">The expected error messages.</param>
    [Theory]
    [MemberData(nameof(ValidationTestData.Email.InvalidEmails), MemberType = typeof(ValidationTestData.Email))]
    public void ValidateUpdateUserCommand_WhenEmailIsInvalid_ShouldHaveValidationError(
        string email,
        IEnumerable<string> expectedErrorMessages
    )
    {
        var command = UpdateUserCommandUtils.CreateCommand(email: email);

        var result = _validator.TestValidate(command);

        result.IsValid.Should().BeFalse();

        foreach (var expectedErrorMessage in expectedErrorMessages)
        {
            result.ShouldHaveValidationErrorFor(c => c.Email).WithErrorMessage(expectedErrorMessage);
        }
    }
}
