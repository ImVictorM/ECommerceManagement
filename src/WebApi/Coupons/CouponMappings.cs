using Application.Coupons.Commands.CreateCoupon;

using Contracts.Coupons;

using Mapster;

namespace WebApi.Coupons;

internal sealed class CouponMappings : IRegister
{
    /// <inheritdoc/>
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<CreateCouponRequest, CreateCouponCommand>();
    }
}
