using Microsoft.Extensions.Logging;

namespace Application.Orders.Queries.GetCustomerOrderById;

internal sealed partial class GetCustomerOrderByIdQueryHandler
{
    private readonly ILogger<GetCustomerOrderByIdQueryHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Starting customer order retrieval." +
        " Order identifier: {OrderId}." +
        " Order owner identifier: {OwnerId}."
    )]
    private partial void LogInitiatingCustomerOrderRetrieval(
        string orderId,
        string ownerId
    );

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "The order could not be retrieved because it does not exist."
    )]
    private partial void LogOrderNotFound();

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message =
        "The order was retrieved. " +
        "Initiating order payment details retrieval."
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
        Message =
        "The customer detailed order was retrieved. " +
        "The operation completed successfully."
    )]
    private partial void LogOrderDetailedRetrievedSuccessfully();
}
