using Application.Products.Commands.UpdateProductInventory;
using Application.UnitTests.Products.Commands.TestUtils;
using Application.UnitTests.TestUtils.ValidationData;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace Application.UnitTests.Products.Commands.UpdateProductInventory;

/// <summary>
/// Tests for the <see cref="UpdateProductInventoryCommandValidator"/> validator.
/// </summary>
public class UpdateProductInventoryCommandValidatorTests
{
    private readonly UpdateProductInventoryCommandValidator _validator;

    /// <summary>
    /// Initiates a new instance of the <see cref="UpdateProductInventoryCommandValidatorTests"/> class.
    /// </summary>
    public UpdateProductInventoryCommandValidatorTests()
    {
        _validator = new UpdateProductInventoryCommandValidator();
    }

    /// <summary>
    /// Tests when the quantity to increment is negative or zero the field has validation errors.
    /// </summary>
    /// <param name="invalidQuantity">The invalid quantity.</param>
    [Theory]
    [MemberData(nameof(ValidationTestData.NonPositiveNumbers), MemberType = typeof(ValidationTestData))]
    public void ValidateUpdateProductInventoryCommand_WhenQuantityIsNotGreaterThanZero_ShouldHaveValidationErrors(int invalidQuantity)
    {
        var command = UpdateProductInventoryCommandUtils.CreateCommand(quantityToAdd: invalidQuantity);

        var result = _validator.TestValidate(command);

        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(c => c.QuantityToIncrement).WithErrorMessage("'Quantity To Increment' must be greater than '0'.");
    }
}
