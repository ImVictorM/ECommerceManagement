using Application.Coupons.Abstracts;
using Application.Coupons.Commands.CreateCoupon;
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
    }
}
