using FluentValidation;

namespace Application.Products.Commands.AddStock;

internal class AddStockCommandValidator : AbstractValidator<AddStockCommand>
{
    public AddStockCommandValidator()
    {
        RuleFor(x => x.QuantityToAdd).GreaterThan(0);
    }
}
