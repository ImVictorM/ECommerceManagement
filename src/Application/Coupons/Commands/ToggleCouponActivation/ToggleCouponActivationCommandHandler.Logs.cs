using Microsoft.Extensions.Logging;

namespace Application.Coupons.Commands.ToggleCouponActivation;

internal sealed partial class ToggleCouponActivationCommandHandler
{
    private readonly ILogger<ToggleCouponActivationCommandHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message =
        "Initiating toggle coupon activation for coupon with identifier '{CouponId}'."
    )]
    private partial void LogInitiatingToggleCouponActivation(string couponId);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "The coupon could not be found. The operation failed."
    )]
    private partial void LogCouponNotFound();

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message = "The current active state of the coupon is {State}."
    )]
    private partial void LogCurrentCouponState(bool state);

    [LoggerMessage(
        EventId = 4,
        Level = LogLevel.Debug,
        Message = "The coupon active state was toggled to {State}."
    )]
    private partial void LogCouponStateToggledTo(bool state);

    [LoggerMessage(
        EventId = 5,
        Level = LogLevel.Debug,
        Message =
        "The coupon state has been changed and saved. " +
        "The operation was completed successfully."
    )]
    private partial void LogCouponStateChangedSuccessfully();
}
