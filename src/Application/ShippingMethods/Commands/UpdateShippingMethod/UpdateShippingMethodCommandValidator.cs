using FluentValidation;

namespace Application.ShippingMethods.Commands.UpdateShippingMethod;

/// <summary>
/// Validates the <see cref="UpdateShippingMethodCommand"/> command inputs.
/// </summary>
public class UpdateShippingMethodCommandValidator : AbstractValidator<UpdateShippingMethodCommand>
{
    /// <summary>
    /// Initiates a new instance of the <see cref="UpdateShippingMethodCommandValidator"/> class.
    /// </summary>
    public UpdateShippingMethodCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0m);
        RuleFor(x => x.EstimatedDeliveryDays).GreaterThanOrEqualTo(1);
    }
}
