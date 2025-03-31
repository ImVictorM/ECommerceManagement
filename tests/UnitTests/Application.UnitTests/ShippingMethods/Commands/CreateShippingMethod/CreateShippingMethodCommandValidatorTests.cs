using Application.ShippingMethods.Commands.CreateShippingMethod;
using Application.UnitTests.ShippingMethods.Commands.TestUtils;
using Application.UnitTests.TestUtils.ValidationData;

using FluentAssertions;
using FluentValidation.TestHelper;

namespace Application.UnitTests.ShippingMethods.Commands.CreateShippingMethod;

/// <summary>
/// Unit tests for the <see cref="CreateShippingMethodCommandValidator"/>
/// command validator.
/// </summary>
public class CreateShippingMethodCommandValidatorTests
{
    private readonly CreateShippingMethodCommandValidator _validator;

    /// <summary>
    /// Initiates a new instance of the
    /// <see cref="CreateShippingMethodCommandValidatorTests"/> class.
    /// </summary>
    public CreateShippingMethodCommandValidatorTests()
    {
        _validator = new CreateShippingMethodCommandValidator();
    }

    /// <summary>
    /// Verifies when the shipping method name is empty the name should contain
    /// validation errors.
    /// </summary>
    /// <param name="empty">The empty name.</param>
    [Theory]
    [MemberData(
        nameof(ValidationTestData.EmptyStrings),
        MemberType = typeof(ValidationTestData)
    )]
    public void ValidateCreateShippingMethodCommand_WithEmptyName_ShouldHaveValidationError(
        string empty
    )
    {
        var command = CreateShippingMethodCommandUtils.CreateCommand(name: empty);

        var result = _validator.TestValidate(command);

        result.IsValid.Should().BeFalse();
        result
            .ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorMessage("'Name' must not be empty.");
    }

    /// <summary>
    /// Verifies when the price of a shipping method is negative the price should
    /// have validation errors.
    /// </summary>
    /// <param name="invalidPrice">The invalid price.</param>
    [Theory]
    [
        MemberData(nameof(ValidationTestData.NegativeNumbers),
        MemberType = typeof(ValidationTestData)
    )]
    public void ValidateCreateShippingMethodCommand_WithNegativePrice_ShouldHaveValidationError(
        int invalidPrice
    )
    {
        var command = CreateShippingMethodCommandUtils.CreateCommand(
            price: invalidPrice
        );

        var result = _validator.TestValidate(command);

        result.IsValid.Should().BeFalse();
        result
            .ShouldHaveValidationErrorFor(c => c.Price)
            .WithErrorMessage("'Price' must be greater than or equal to '0'.");
    }

    /// <summary>
    /// Verifies when the estimated delivery days of a shipping method is not greater
    /// than or equal to 1 the estimated delivery days should have validation errors.
    /// </summary>
    /// <param name="invalidEstimatedDeliveryDays">
    /// The invalid estimated delivery days.
    /// </param>
    [Theory]
    [MemberData(
        nameof(ValidationTestData.NonPositiveNumbers),
        MemberType = typeof(ValidationTestData)
    )]
    public void ValidateCreateShippingMethodCommand_WithNegativeEstimatedDeliveryDays_ShouldHaveValidationError(
        int invalidEstimatedDeliveryDays
    )
    {
        var command = CreateShippingMethodCommandUtils.CreateCommand(
            estimatedDeliveryDays: invalidEstimatedDeliveryDays
        );

        var result = _validator.TestValidate(command);

        result.IsValid.Should().BeFalse();
        result
            .ShouldHaveValidationErrorFor(c => c.EstimatedDeliveryDays)
            .WithErrorMessage(
                "'Estimated Delivery Days' must be greater than or equal to '1'."
            );
    }
}
