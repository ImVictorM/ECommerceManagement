using Application.Coupons.Commands.CreateCoupon;
using Application.Coupons.Commands.UpdateCoupon;
using Application.Coupons.DTOs.Restrictions;
using Application.Coupons.DTOs.Results;

using Contracts.Coupons;
using Contracts.Coupons.Restrictions;

using Mapster;

namespace WebApi.Coupons;

internal sealed class CouponMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<CouponRestriction, CouponRestrictionIO>()
            .TwoWays()
            .Include<CouponProductRestriction, CouponProductRestrictionIO>()
            .Include<CouponCategoryRestriction, CouponCategoryRestrictionIO>();

        config.NewConfig<CreateCouponRequest, CreateCouponCommand>();

        config
            .NewConfig<(string Id, UpdateCouponRequest Request), UpdateCouponCommand>()
            .Map(dest => dest.CouponId, src => src.Id)
            .Map(dest => dest, src => src.Request);

        config.NewConfig<CouponResult, CouponResponse>();
    }
}
