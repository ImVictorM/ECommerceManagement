using Application.Authentication.Queries.Login;
using Application.UnitTests.Authentication.Queries.TestUtils;
using Application.UnitTests.TestUtils.ValidationData;

using FluentAssertions;
using FluentValidation.TestHelper;

namespace Application.UnitTests.Authentication.Queries.Login;

/// <summary>
/// Tests for the <see cref="LoginQueryValidator"/> validator;
/// </summary>
public class LoginQueryValidatorTests
{
    private readonly LoginQueryValidator _validator;

    /// <summary>
    /// Initiates a new instance of the <see cref="LoginQueryValidatorTests"/> class.
    /// </summary>
    public LoginQueryValidatorTests()
    {
        _validator = new LoginQueryValidator();
    }

    /// <summary>
    /// Tests when the email is empty the email has validation errors.
    /// </summary>
    /// <param name="empty">An empty input string.</param>
    [Theory]
    [MemberData(nameof(ValidationTestData.EmptyStrings), MemberType = typeof(ValidationTestData))]
    public void ValidateLoginQuery_WhenEmailIsEmpty_ShouldHaveValidationError(string empty)
    {
        var command = LoginQueryUtils.CreateQuery(email: empty);

        var result = _validator.TestValidate(command);

        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(c => c.Email).WithErrorMessage("'Email' must not be empty.");
    }

    /// <summary>
    /// Tests when the password is empty the password has validation errors.
    /// </summary>
    /// <param name="empty">An empty input string.</param>
    [Theory]
    [MemberData(nameof(ValidationTestData.EmptyStrings), MemberType = typeof(ValidationTestData))]
    public void ValidateLoginQuery_WhenPasswordIsEmpty_ShouldHaveValidationError(string empty)
    {
        var command = LoginQueryUtils.CreateQuery(password: empty);

        var result = _validator.TestValidate(command);

        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(c => c.Password).WithErrorMessage("'Password' must not be empty.");
    }
}
