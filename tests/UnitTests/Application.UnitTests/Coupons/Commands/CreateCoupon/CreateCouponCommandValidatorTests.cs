using Application.Coupons.Commands.CreateCoupon;
using Application.UnitTests.Coupons.Commands.TestUtils;
using Application.UnitTests.TestUtils.ValidationData;

using FluentAssertions;
using FluentValidation.TestHelper;

namespace Application.UnitTests.Coupons.Commands.CreateCoupon;

/// <summary>
/// Unit tests for the <see cref="CreateCouponCommandValidator"/> class.
/// </summary>
public class CreateCouponCommandValidatorTests
{
    private readonly CreateCouponCommandValidator _validator;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="CreateCouponCommandValidatorTests"/> class.
    /// </summary>
    public CreateCouponCommandValidatorTests()
    {
        _validator = new CreateCouponCommandValidator();
    }

    /// <summary>
    /// Verifies creating the command with empty code should
    /// have validation errors.
    /// </summary>
    /// <param name="emptyCode">The empty code.</param>
    [Theory]
    [MemberData(
        nameof(ValidationTestData.EmptyStrings),
        MemberType = typeof(ValidationTestData)
    )]
    public void ValidateCreateCouponCommand_WhenCodeIsEmpty_ShouldHaveValidationErrors(
        string emptyCode
    )
    {
        var command = CreateCouponCommandUtils.CreateCommand(code: emptyCode);

        var result = _validator.TestValidate(command);

        result.IsValid.Should().BeFalse();
        result
            .ShouldHaveValidationErrorFor(c => c.Code)
            .WithErrorMessage("'Code' must not be empty.");
    }

    /// <summary>
    /// Verifies creating the command with non-positive usage limit should
    /// have validation errors.
    /// </summary>
    /// <param name="nonPositiveUsageLimit">
    /// The non-positive usage limit.
    /// </param>
    [Theory]
    [MemberData(
        nameof(ValidationTestData.NonPositiveNumbers),
        MemberType = typeof(ValidationTestData)
    )]
    public void ValidateCreateCouponCommand_WithLessThanOneUsageLimit_ShouldHaveValidationErrors(
        int nonPositiveUsageLimit
    )
    {
        var command = CreateCouponCommandUtils.CreateCommand(
            usageLimit: nonPositiveUsageLimit
        );

        var result = _validator.TestValidate(command);

        result.IsValid.Should().BeFalse();
        result
            .ShouldHaveValidationErrorFor(c => c.UsageLimit)
            .WithErrorMessage("'Usage Limit' must be greater than or equal to '1'.");
    }

    /// <summary>
    /// Verifies creating the command with negative minimum price should
    /// have validation errors.
    /// </summary>
    /// <param name="negativePrice">The negative price.</param>
    [Theory]
    [MemberData(
        nameof(ValidationTestData.NegativeNumbers),
        MemberType = typeof(ValidationTestData)
    )]
    public void ValidateCreateCouponCommand_WithNegativeMinPrice_ShouldHaveValidationErrors(
        decimal negativePrice
    )
    {
        var command = CreateCouponCommandUtils.CreateCommand(
            minPrice: negativePrice
        );

        var result = _validator.TestValidate(command);

        result.IsValid.Should().BeFalse();
        result
            .ShouldHaveValidationErrorFor(c => c.MinPrice)
            .WithErrorMessage("'Min Price' must be greater than or equal to '0'.");
    }
}
