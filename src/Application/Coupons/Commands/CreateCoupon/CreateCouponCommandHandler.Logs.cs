using Microsoft.Extensions.Logging;

namespace Application.Coupons.Commands.CreateCoupon;

internal sealed partial class CreateCouponCommandHandler
{
    private readonly ILogger<CreateCouponCommandHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Initiating coupon creation with code {Code}."
    )]
    private partial void LogInitiatingCouponCreation(string code);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message =
        "The coupon object was created. " +
        "Initiating coupon restrictions parse."
    )]
    private partial void LogCouponCreated();

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message =
        "The restrictions were parsed successfully. " +
        "Total of restrictions {RestrictionsCount}."
    )]
    private partial void LogRestrictionsParsed(int restrictionsCount);

    [LoggerMessage(
        EventId = 4,
        Level = LogLevel.Debug,
        Message = "Assigning restriction of type {RestrictionType}."
    )]
    private partial void LogAssigningRestriction(string restrictionType);

    [LoggerMessage(
        EventId = 5,
        Level = LogLevel.Information,
        Message = "Coupon created and saved successfully with id {CouponId}."
    )]
    private partial void LogCouponCreatedAndSavedSuccessfully(string couponId);
}
