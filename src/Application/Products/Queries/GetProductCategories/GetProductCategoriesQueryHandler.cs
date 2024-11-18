using Application.Products.Queries.Common.DTOs;
using Domain.ProductAggregate.Enumerations;
using MediatR;

namespace Application.Products.Queries.GetProductCategories;

/// <summary>
/// Handler the <see cref="GetProductCategoriesQuery"/> query.
/// </summary>
public class GetProductCategoriesQueryHandler : IRequestHandler<GetProductCategoriesQuery, ProductCategoriesResult>
{
    /// <inheritdoc/>
    public Task<ProductCategoriesResult> Handle(GetProductCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = Category.List();

        return Task.FromResult(new ProductCategoriesResult(categories));
    }
}
