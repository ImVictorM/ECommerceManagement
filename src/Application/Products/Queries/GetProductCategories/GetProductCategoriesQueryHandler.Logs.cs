using Microsoft.Extensions.Logging;

namespace Application.Products.Queries.GetProductCategories;

public partial class GetProductCategoriesQueryHandler
{
    private readonly ILogger<GetProductCategoriesQueryHandler> _logger;

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Listing the product categories."
    )]
    private partial void LogListingCategories();

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "Categories listed successfully. Returning them."
    )]
    private partial void LogReturningCategories();
}
