using Application.Coupons.Abstracts;
using Application.Coupons.Commands.CreateCoupon;
using Application.Coupons.Commands.UpdateCoupon;
using Application.Coupons.DTOs;

using Contracts.Coupons;
using Contracts.Coupons.Restrictions;

using Mapster;

namespace WebApi.Coupons;

internal sealed class CouponMappings : IRegister
{
    /// <inheritdoc/>
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<CouponRestriction, ICouponRestrictionInput>()
            .Include<ProductRestriction, ProductRestrictionInput>()
            .Include<CategoryRestriction, CategoryRestrictionInput>();

        config.NewConfig<CreateCouponRequest, CreateCouponCommand>();

        config
            .NewConfig<(string Id, UpdateCouponRequest Request), UpdateCouponCommand>()
            .Map(dest => dest.CouponId, src => src.Id)
            .Map(dest => dest, src => src.Request);

        config
            .NewConfig<CouponResult, CouponResponse>()
            .Map(dest => dest.Id, src => src.Coupon.Id.ToString())
            .Map(dest => dest, src => src.Coupon);
    }
}
