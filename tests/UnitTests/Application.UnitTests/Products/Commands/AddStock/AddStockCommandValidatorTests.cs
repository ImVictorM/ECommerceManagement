using Application.Products.Commands.AddStock;
using Application.UnitTests.Products.Commands.TestUtils;
using Application.UnitTests.TestUtils.ValidationData;

using FluentAssertions;
using FluentValidation.TestHelper;

namespace Application.UnitTests.Products.Commands.AddStock;

/// <summary>
/// Unit tests for the <see cref="AddStockCommandValidator"/> validator.
/// </summary>
public class AddStockCommandValidatorTests
{
    private readonly AddStockCommandValidator _validator;

    /// <summary>
    /// Initiates a new instance of the
    /// <see cref="AddStockCommandValidatorTests"/> class.
    /// </summary>
    public AddStockCommandValidatorTests()
    {
        _validator = new AddStockCommandValidator();
    }

    /// <summary>
    /// Verifies when the quantity to add is non-positive the field has
    /// a validation error.
    /// </summary>
    /// <param name="invalidQuantity">The invalid quantity.</param>
    [Theory]
    [MemberData(
        nameof(ValidationTestData.NonPositiveNumbers),
        MemberType = typeof(ValidationTestData)
    )]
    public void ValidateAddStockCommand_WithNonPositiveQuantity_ShouldHaveValidationError(
        int invalidQuantity
    )
    {
        var command = AddStockCommandUtils.CreateCommand(
            quantityToAdd: invalidQuantity
        );

        var result = _validator.TestValidate(command);

        result.IsValid.Should().BeFalse();
        result
            .ShouldHaveValidationErrorFor(c => c.QuantityToAdd)
            .WithErrorMessage("'Quantity To Add' must be greater than '0'.");
    }
}
