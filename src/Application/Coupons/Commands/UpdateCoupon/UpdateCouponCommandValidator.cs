using FluentValidation;

namespace Application.Coupons.Commands.UpdateCoupon;

internal class UpdateCouponCommandValidator : AbstractValidator<UpdateCouponCommand>
{
    public UpdateCouponCommandValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.UsageLimit).GreaterThanOrEqualTo(1);
        RuleFor(x => x.MinPrice).GreaterThanOrEqualTo(0);
    }
}
