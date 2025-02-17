using Application.ShippingMethods.Commands.UpdateShippingMethod;
using Application.UnitTests.ShippingMethods.Commands.TestUtils;
using Application.UnitTests.TestUtils.ValidationData;

using FluentAssertions;
using FluentValidation.TestHelper;

namespace Application.UnitTests.ShippingMethods.Commands.UpdateShippingMethod;

/// <summary>
/// Unit tests for the <see cref="UpdateShippingMethodCommandValidator"/> command validator.
/// </summary>
public class UpdateShippingMethodCommandValidatorTests
{
    private readonly UpdateShippingMethodCommandValidator _validator;

    /// <summary>
    /// Initiates a new instance of the <see cref="UpdateShippingMethodCommandValidatorTests"/> class.
    /// </summary>
    public UpdateShippingMethodCommandValidatorTests()
    {
        _validator = new UpdateShippingMethodCommandValidator();
    }

    /// <summary>
    /// Tests when the shipping method name is empty the name should contain validation errors.
    /// </summary>
    /// <param name="empty">The empty name.</param>
    [Theory]
    [MemberData(nameof(ValidationTestData.EmptyStrings), MemberType = typeof(ValidationTestData))]
    public void ValidateUpdateShippingMethodCommand_WhenNameIsEmpty_ShouldHaveValidationErrors(string empty)
    {
        var command = UpdateShippingMethodCommandUtils.CreateCommand(name: empty);

        var result = _validator.TestValidate(command);

        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(c => c.Name).WithErrorMessage("'Name' must not be empty.");
    }

    /// <summary>
    /// Tests when the price of a shipping method is not greater than or equal to zero the price should have validation errors.
    /// </summary>
    /// <param name="invalidPrice">The invalid price.</param>
    [Theory]
    [MemberData(nameof(ValidationTestData.NegativeNumbers), MemberType = typeof(ValidationTestData))]
    public void ValidateUpdateShippingMethodCommand_WhenPriceIsNotGreaterThanOrEqualToZero_ShouldHaveValidationErrors(int invalidPrice)
    {
        var command = UpdateShippingMethodCommandUtils.CreateCommand(price: invalidPrice);

        var result = _validator.TestValidate(command);

        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(c => c.Price).WithErrorMessage("'Price' must be greater than or equal to '0'.");
    }

    /// <summary>
    /// Tests when the estimated delivery days of a shipping method is not greater than or equal to 1 the estimated delivery days should have validation errors.
    /// </summary>
    /// <param name="invalidEstimatedDeliveryDays">The invalid estimated delivery days.</param>
    [Theory]
    [MemberData(nameof(ValidationTestData.NonPositiveNumbers), MemberType = typeof(ValidationTestData))]
    public void ValidateUpdateShippingMethodCommand_WhenEstimatedDeliveryDaysIsNotGreaterThanOrEqualToOne_ShouldHaveValidationErrors(int invalidEstimatedDeliveryDays)
    {
        var command = UpdateShippingMethodCommandUtils.CreateCommand(estimatedDeliveryDays: invalidEstimatedDeliveryDays);

        var result = _validator.TestValidate(command);

        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(c => c.EstimatedDeliveryDays).WithErrorMessage("'Estimated Delivery Days' must be greater than or equal to '1'.");
    }
}
