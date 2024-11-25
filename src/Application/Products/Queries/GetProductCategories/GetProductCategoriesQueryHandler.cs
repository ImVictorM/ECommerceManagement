using Application.Products.Queries.Common.DTOs;
using Domain.ProductAggregate.Enumerations;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Products.Queries.GetProductCategories;

/// <summary>
/// Handler the <see cref="GetProductCategoriesQuery"/> query.
/// </summary>
public sealed partial class GetProductCategoriesQueryHandler : IRequestHandler<GetProductCategoriesQuery, ProductCategoriesResult>
{
    /// <summary>
    /// Initiates a new instance of the <see cref="GetProductCategoriesQueryHandler"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public GetProductCategoriesQueryHandler(ILogger<GetProductCategoriesQueryHandler> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc/>
    public Task<ProductCategoriesResult> Handle(GetProductCategoriesQuery request, CancellationToken cancellationToken)
    {
        LogListingCategories();

        var categories = Category.List();

        LogReturningCategories();
        return Task.FromResult(new ProductCategoriesResult(categories));
    }
}
