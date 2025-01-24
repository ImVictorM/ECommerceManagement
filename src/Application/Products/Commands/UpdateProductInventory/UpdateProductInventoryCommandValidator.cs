using FluentValidation;

namespace Application.Products.Commands.UpdateProductInventory;

/// <summary>
/// Validator for the <see cref="UpdateProductInventoryCommand"/> command.
/// </summary>
public class UpdateProductInventoryCommandValidator : AbstractValidator<UpdateProductInventoryCommand>
{
    /// <summary>
    /// Initiates a new instance of the <see cref="UpdateProductInventoryCommandValidator"/> class.
    /// </summary>
    public UpdateProductInventoryCommandValidator()
    {
        RuleFor(x => x.QuantityToIncrement).GreaterThan(0);
    }
}
