using Microsoft.Extensions.Logging;

namespace Application.Coupons.Commands.DeleteCoupon;

internal sealed partial class DeleteCouponCommandHandler
{
    private readonly ILogger<DeleteCouponCommandHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Initiating deletion of coupon with identifier {CouponId}."
    )]
    private partial void LogInitiatingCouponDeletion(string couponId);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "The coupon could not be found. The operation failed."
    )]
    private partial void LogCouponNotFound();

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message = "The coupon was deleted. The operation completed successfully."
    )]
    private partial void LogCouponDeletedSuccessfully();
}
