using Microsoft.Extensions.Logging;

namespace Application.Coupons.Commands.UpdateCoupon;

internal sealed partial class UpdateCouponCommandHandler
{
    private readonly ILogger<UpdateCouponCommandHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Initiating update for coupon with id {CouponId}"
    )]
    private partial void LogInitiatingCouponUpdate(string couponId);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "The coupon was not found. Operation failed."
    )]
    private partial void LogCouponNotFound();

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message = "The coupon base info was updated."
    )]
    private partial void LogCouponUpdated();

    [LoggerMessage(
        EventId = 4,
        Level = LogLevel.Debug,
        Message = "All the coupon former restrictions were cleared."
    )]
    private partial void LogCouponFormerRestrictionsCleared();

    [LoggerMessage(
        EventId = 5,
        Level = LogLevel.Debug,
        Message = "Assigning new restriction of type {RestrictionType}."
    )]
    private partial void LogAssigningNewRestriction(string restrictionType);

    [LoggerMessage(
        EventId = 6,
        Level = LogLevel.Debug,
        Message = "All new restrictions assigned successfully."
    )]
    private partial void LogRestrictionsAssigned();

    [LoggerMessage(
        EventId = 7,
        Level = LogLevel.Debug,
        Message =
        "The coupon information and restrictions were updated." +
        " Operation complete successfully."
    )]
    private partial void LogCouponUpdatedSuccessfully();
}
