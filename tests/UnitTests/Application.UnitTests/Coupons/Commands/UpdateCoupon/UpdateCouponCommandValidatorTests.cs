using Application.Coupons.Commands.UpdateCoupon;
using Application.UnitTests.Coupons.Commands.TestUtils;
using Application.UnitTests.TestUtils.ValidationData;

using FluentAssertions;
using FluentValidation.TestHelper;

namespace Application.UnitTests.Coupons.Commands.UpdateCoupon;

/// <summary>
/// Unit tests for the <see cref="UpdateCouponCommandValidator"/> class.
/// </summary>
public class UpdateCouponCommandValidatorTests
{
    private readonly UpdateCouponCommandValidator _validator;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="UpdateCouponCommandValidatorTests"/> class.
    /// </summary>
    public UpdateCouponCommandValidatorTests()
    {
        _validator = new UpdateCouponCommandValidator();
    }

    /// <summary>
    /// Verifies updating the coupon with an empty code should
    /// have validation errors.
    /// </summary>
    /// <param name="emptyCode">The empty code.</param>
    [Theory]
    [MemberData(
        nameof(ValidationTestData.EmptyStrings),
        MemberType = typeof(ValidationTestData)
    )]
    public void ValidateUpdateCouponCommand_WithEmptyCode_ShouldHaveValidationErrors(
        string emptyCode
    )
    {
        var command = UpdateCouponCommandUtils.CreateCommand(code: emptyCode);

        var result = _validator.TestValidate(command);

        result.IsValid.Should().BeFalse();
        result
            .ShouldHaveValidationErrorFor(c => c.Code)
            .WithErrorMessage("'Code' must not be empty.");
    }

    /// <summary>
    /// Verifies updating the coupon with a non-positive usage limit should
    /// have validation errors.
    /// </summary>
    /// <param name="nonPositiveUsageLimit">The non-positive usage limit.</param>
    [Theory]
    [MemberData(
        nameof(ValidationTestData.NonPositiveNumbers),
        MemberType = typeof(ValidationTestData)
    )]
    public void ValidateUpdateCouponCommand_WithLessThanOneUsageLimit_ShouldHaveValidationErrors(
        int nonPositiveUsageLimit
    )
    {
        var command = UpdateCouponCommandUtils.CreateCommand(
            usageLimit: nonPositiveUsageLimit
        );

        var result = _validator.TestValidate(command);

        result.IsValid.Should().BeFalse();
        result
            .ShouldHaveValidationErrorFor(c => c.UsageLimit)
            .WithErrorMessage("'Usage Limit' must be greater than or equal to '1'.");
    }

    /// <summary>
    /// Verifies updating the coupon with a negative minimum price should
    /// have validation errors.
    /// </summary>
    /// <param name="negativePrice">The negative price.</param>
    [Theory]
    [MemberData(
        nameof(ValidationTestData.NegativeNumbers),
        MemberType = typeof(ValidationTestData)
    )]
    public void ValidateUpdateCouponCommand_WithNegativeMinPrice_ShouldHaveValidationErrors(
        decimal negativePrice
    )
    {
        var command = UpdateCouponCommandUtils.CreateCommand(
            minPrice: negativePrice
        );

        var result = _validator.TestValidate(command);

        result.IsValid.Should().BeFalse();
        result
            .ShouldHaveValidationErrorFor(c => c.MinPrice)
            .WithErrorMessage("'Min Price' must be greater than or equal to '0'.");
    }
}
