using FluentValidation;

namespace Application.Products.Commands.UpdateProductInventory;

internal class UpdateProductInventoryCommandValidator : AbstractValidator<UpdateProductInventoryCommand>
{
    public UpdateProductInventoryCommandValidator()
    {
        RuleFor(x => x.QuantityToIncrement).GreaterThan(0);
    }
}
