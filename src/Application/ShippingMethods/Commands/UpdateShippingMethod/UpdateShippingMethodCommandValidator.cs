using FluentValidation;

namespace Application.ShippingMethods.Commands.UpdateShippingMethod;

internal class UpdateShippingMethodCommandValidator
    : AbstractValidator<UpdateShippingMethodCommand>
{
    public UpdateShippingMethodCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0m);
        RuleFor(x => x.EstimatedDeliveryDays).GreaterThanOrEqualTo(1);
    }
}
