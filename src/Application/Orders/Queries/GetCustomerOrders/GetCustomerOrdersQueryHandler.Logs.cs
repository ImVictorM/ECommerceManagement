using Microsoft.Extensions.Logging;

namespace Application.Orders.Queries.GetCustomerOrders;

public partial class GetCustomerOrdersQueryHandler
{
    private readonly ILogger<GetCustomerOrdersQueryHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Initiating customer orders retrieval. Order owner id: {OrderOwnerId}."
    )]
    private partial void LogInitiatingOrdersRetrieval(string orderOwnerId);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "Orders retrieved successfully. Quantity of orders found: {QuantityFound}."
    )]
    private partial void LogOrdersRetrievedSuccessfully(int quantityFound);
}
