using Microsoft.Extensions.Logging;

namespace Application.Coupons.Commands.ToggleCouponActivation;

internal sealed partial class ToggleCouponActivationCommandHandler
{
    private readonly ILogger<ToggleCouponActivationCommandHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Initiating toggle coupon activation for coupon with id {CouponId}."
    )]
    private partial void LogInitiatingToggleCouponActivation(string couponId);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "Coupon not found. Operation failed."
    )]
    private partial void LogCouponNotFound();

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message = "The current state of the coupon is {State}."
    )]
    private partial void LogCurrentCouponState(bool state);

    [LoggerMessage(
        EventId = 4,
        Level = LogLevel.Debug,
        Message = "The coupon state was toggled to {State}."
    )]
    private partial void LogCouponStateToggledTo(bool state);

    [LoggerMessage(
        EventId = 5,
        Level = LogLevel.Debug,
        Message =
        "The coupon state was changed and saved. Operation complete successfully."
    )]
    private partial void LogCouponStateChangedSuccessfully();
}
