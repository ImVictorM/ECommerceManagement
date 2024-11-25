using Microsoft.Extensions.Logging;

namespace Application.Products.Queries.GetProducts;

public partial class GetProductsQueryHandler
{
    private readonly ILogger<GetProductsQueryHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Initiating retrieval of products."
    )]
    private partial void LogInitiatedRetrievingProducts();

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "The products was fetched successfully.\nLimit {Limit}.\nCategories: {Categories}."
    )]
    private partial void LogProductsRetrievedSuccessfully(int limit, string categories);
}
