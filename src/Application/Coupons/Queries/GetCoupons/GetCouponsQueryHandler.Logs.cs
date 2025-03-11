using Microsoft.Extensions.Logging;

namespace Application.Coupons.Queries.GetCoupons;

internal sealed partial class GetCouponsQueryHandler
{
    private readonly ILogger<GetCouponsQueryHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message =
        "Initiating retrieval of coupons with filters: " +
        "Active={Active}, " +
        "ExpiringAfter={ExpiringAfter}, " +
        "ExpiringBefore={ExpiringBefore}, " +
        "ValidForDate={ValidForDate}"
    )]
    private partial void LogInitiatingCouponsRetrieval(
        bool? active,
        DateTimeOffset? expiringAfter,
        DateTimeOffset? expiringBefore,
        DateTimeOffset? validForDate
    );

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "{Count} coupons were retrieved. Operation complete successfully."
    )]
    private partial void LogCouponsRetrievedSuccessfully(int count);
}
