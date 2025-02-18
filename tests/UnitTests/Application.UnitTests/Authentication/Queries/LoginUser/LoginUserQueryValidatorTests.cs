using Application.Authentication.Queries.LoginUser;
using Application.UnitTests.Authentication.Queries.TestUtils;
using Application.UnitTests.TestUtils.ValidationData;

using FluentAssertions;
using FluentValidation.TestHelper;

namespace Application.UnitTests.Authentication.Queries.LoginUser;

/// <summary>
/// Tests for the <see cref="LoginUserQueryValidator"/> validator;
/// </summary>
public class LoginUserQueryValidatorTests
{
    private readonly LoginUserQueryValidator _validator;

    /// <summary>
    /// Initiates a new instance of the <see cref="LoginUserQueryValidatorTests"/> class.
    /// </summary>
    public LoginUserQueryValidatorTests()
    {
        _validator = new LoginUserQueryValidator();
    }

    /// <summary>
    /// Tests when the email is empty the email has validation errors.
    /// </summary>
    /// <param name="empty">An empty input string.</param>
    [Theory]
    [MemberData(nameof(ValidationTestData.EmptyStrings), MemberType = typeof(ValidationTestData))]
    public void ValidateLoginUserQuery_WhenEmailIsEmpty_ShouldHaveValidationError(string empty)
    {
        var command = LoginUserQueryUtils.CreateQuery(email: empty);

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
    public void ValidateLoginUserQuery_WhenPasswordIsEmpty_ShouldHaveValidationError(string empty)
    {
        var command = LoginUserQueryUtils.CreateQuery(password: empty);

        var result = _validator.TestValidate(command);

        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(c => c.Password).WithErrorMessage("'Password' must not be empty.");
    }
}
