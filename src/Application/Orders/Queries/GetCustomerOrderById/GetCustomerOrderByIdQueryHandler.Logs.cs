using Microsoft.Extensions.Logging;

namespace Application.Orders.Queries.GetCustomerOrderById;

public partial class GetCustomerOrderByIdQueryHandler
{
    private readonly ILogger<GetCustomerOrderByIdQueryHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Initiating order fetch for order with id {OrderId} and owner id {OwnerId}"
    )]
    private partial void LogHandlingOrderFetch(string orderId, string ownerId);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "The order could not be found"
    )]
    private partial void LogOrderNotFound();

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message = "The order was found. Initiating order payment retrieval"
    )]
    private partial void LogOrderFound();

    [LoggerMessage(
        EventId = 4,
        Level = LogLevel.Debug,
        Message = "The order payment was not found internally"
    )]
    private partial void LogOrderPaymentNotFound();

    [LoggerMessage(
        EventId = 5,
        Level = LogLevel.Debug,
        Message = "The order payment was found internally. Initiating payment details retrieval using gateway"
    )]
    private partial void LogOrderPaymentFound();

    [LoggerMessage(
        EventId = 6,
        Level = LogLevel.Debug,
        Message = "The order payment details was fetched successfully"
    )]
    private partial void LogPaymentDetailsFetched();

    [LoggerMessage(
        EventId = 7,
        Level = LogLevel.Debug,
        Message = "Operation complete. Returning results"
    )]
    private partial void LogReturningResult();
}
