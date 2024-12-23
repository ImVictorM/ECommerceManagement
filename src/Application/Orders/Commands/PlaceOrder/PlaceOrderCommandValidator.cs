using FluentValidation;

namespace Application.Orders.Commands.PlaceOrder;

/// <summary>
/// Validates the <see cref="PlaceOrderCommand"/> command input values.
/// </summary>
public class PlaceOrderCommandValidator : AbstractValidator<PlaceOrderCommand>
{
    /// <summary>
    /// Initiates a new instance of the <see cref="PlaceOrderCommandValidator"/> class.
    /// </summary>
    public PlaceOrderCommandValidator()
    {
        RuleFor(x => x.Products).NotEmpty();
        RuleFor(x => x.CurrentUserId).NotEmpty();
        RuleFor(x => x.Installments).GreaterThan(0);
    }
}
