using Microsoft.Extensions.Logging;

namespace Application.Orders.Queries.GetOrderById;

public sealed partial class GetOrderByIdQueryHandler
{
    private readonly ILogger<GetOrderByIdQueryHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Starting order retrieval." +
        " Order identifier: {OrderId}."
    )]
    private partial void LogInitiatingOrderRetrieval(string orderId);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "The order could not be retrieved because it does not exist."
    )]
    private partial void LogOrderNotFound();

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message = "The order was retrieved. Initiating order payment details retrieval."
    )]
    private partial void LogOrderRetrieved();

    [LoggerMessage(
        EventId = 4,
        Level = LogLevel.Debug,
        Message = "The order payment details was fetched successfully."
    )]
    private partial void LogOrderPaymentDetailsRetrieved();

    [LoggerMessage(
        EventId = 5,
        Level = LogLevel.Debug,
        Message = "The detailed order was retrieved. The operation complete successfully."
    )]
    private partial void LogOrderDetailedRetrievedSuccessfully();
}
