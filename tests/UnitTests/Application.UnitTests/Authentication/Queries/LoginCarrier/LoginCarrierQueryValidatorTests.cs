using Application.Authentication.Queries.LoginCarrier;
using Application.UnitTests.Authentication.Queries.TestUtils;
using Application.UnitTests.TestUtils.ValidationData;

using FluentAssertions;
using FluentValidation.TestHelper;

namespace Application.UnitTests.Authentication.Queries.LoginCarrier;

/// <summary>
/// Unit tests for the <see cref="LoginCarrierQueryValidatorTests"/> validator.
/// </summary>
public class LoginCarrierQueryValidatorTests
{
    private readonly LoginCarrierQueryValidator _validator;

    /// <summary>
    /// Initiates a new instance of the <see cref="LoginCarrierQueryValidatorTests"/> class.
    /// </summary>
    public LoginCarrierQueryValidatorTests()
    {
        _validator = new LoginCarrierQueryValidator();
    }

    /// <summary>
    /// Verifies when the query email is empty the email has validation errors.
    /// </summary>
    /// <param name="empty">An empty input string.</param>
    [Theory]
    [MemberData(nameof(ValidationTestData.EmptyStrings), MemberType = typeof(ValidationTestData))]
    public void ValidateLoginCarrierQuery_WhenEmailIsEmpty_ShouldHaveValidationError(string empty)
    {
        var command = LoginCarrierQueryUtils.CreateQuery(email: empty);

        var result = _validator.TestValidate(command);

        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(c => c.Email).WithErrorMessage("'Email' must not be empty.");
    }

    /// <summary>
    /// Verifies when the query password is empty the password has validation errors.
    /// </summary>
    /// <param name="empty">An empty input string.</param>
    [Theory]
    [MemberData(nameof(ValidationTestData.EmptyStrings), MemberType = typeof(ValidationTestData))]
    public void ValidateLoginCarrierQuery_WhenPasswordIsEmpty_ShouldHaveValidationError(string empty)
    {
        var command = LoginCarrierQueryUtils.CreateQuery(password: empty);

        var result = _validator.TestValidate(command);

        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(c => c.Password).WithErrorMessage("'Password' must not be empty.");
    }
}
