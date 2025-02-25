using FluentValidation;

namespace Application.Orders.Commands.PlaceOrder;

internal class PlaceOrderCommandValidator : AbstractValidator<PlaceOrderCommand>
{
    public PlaceOrderCommandValidator()
    {
        RuleFor(x => x.Products).NotEmpty();
        RuleFor(x => x.Installments).GreaterThan(0);
    }
}
