using Application.Orders.Commands.PlaceOrder;
using Application.UnitTests.Orders.Commands.TestUtils;

using FluentValidation.TestHelper;

namespace Application.UnitTests.Orders.Commands.PlaceOrder;

/// <summary>
/// Tests for the <see cref="PlaceOrderCommandValidator"/> validator.
/// </summary>
public class PlaceOrderCommandValidatorTests
{
    private readonly PlaceOrderCommandValidator _validator;

    /// <summary>
    /// Initiates a new instance of the <see cref="PlaceOrderCommandValidatorTests"/> class.
    /// </summary>
    public PlaceOrderCommandValidatorTests()
    {
        _validator = new PlaceOrderCommandValidator();
    }

    /// <summary>
    /// Tests when the product list are empty the validator should have an error.
    /// </summary>
    [Fact]
    public void ValidatePlaceOrderCommand_WhenProductsAreEmpty_ShouldHaveValidationError()
    {
        var command = PlaceOrderCommandUtils.CreateCommand(products: []);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Products).WithErrorMessage("'Products' must not be empty.");
    }

    /// <summary>
    /// Tests when the installments are less than zero the validator should have an error.
    /// </summary>
    /// <param name="installments">The negative installment quantity.</param>
    [Theory]
    [InlineData(-1)]
    [InlineData(-55)]
    public void ValidatePlaceOrderCommand_WhenInstallmentsAreZeroOrLess_ShouldHaveValidationError(int installments)
    {
        var command = PlaceOrderCommandUtils.CreateCommand(installments: installments);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Installments).WithErrorMessage("'Installments' must be greater than '0'.");
    }
}
