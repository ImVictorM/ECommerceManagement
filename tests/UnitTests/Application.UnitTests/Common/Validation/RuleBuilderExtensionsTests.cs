using Application.UnitTests.TestUtils.ValidationData;
using static Application.UnitTests.Common.Validation.TestUtils.RuleBuilderUtils;

using FluentValidation.TestHelper;

namespace Application.UnitTests.Common.Validation;

/// <summary>
/// Unit tests for the <see cref="RuleBuilderExtensionsTests"/> extensions.
/// </summary>
public class RuleBuilderExtensionsTests
{
    /// <summary>
    /// Verifies validating with invalid emails should contain validation errors.
    /// </summary>
    /// <param name="email">The invalid email.</param>
    /// <param name="expectedErrors">The expected errors for the current email.</param>
    [Theory]
    [MemberData(nameof(ValidationTestData.Email.InvalidEmails), MemberType = typeof(ValidationTestData.Email))]
    public void ValidateEmail_WithInvalidEmail_ReturnsExpectedErrors(
        string email,
        IReadOnlyList<string> expectedErrors
    )
    {
        var validator = new TestEmailValidator();
        var input = new TestEmailInput { Email = email };

        var result = validator.TestValidate(input);

        foreach (var error in expectedErrors)
        {
            result.ShouldHaveValidationErrorFor(x => x.Email).WithErrorMessage(error);
        }
    }

    /// <summary>
    /// Verifies validating with invalid user names should contain validation errors.
    /// </summary>
    /// <param name="name">The invalid name.</param>
    /// <param name="expectedErrors">The expected errors for the current name.</param>
    [Theory]
    [MemberData(nameof(ValidationTestData.Name.InvalidNames), MemberType = typeof(ValidationTestData.Name))]
    public void ValidateUserName_WithInvalidName_ReturnsExpectedErrors(
        string name,
        IReadOnlyList<string> expectedErrors
    )
    {
        var validator = new TestNameValidator();
        var input = new TestNameInput { Name = name };

        var result = validator.TestValidate(input);

        foreach (var error in expectedErrors)
        {
            result.ShouldHaveValidationErrorFor(x => x.Name).WithErrorMessage(error);
        }
    }
}
