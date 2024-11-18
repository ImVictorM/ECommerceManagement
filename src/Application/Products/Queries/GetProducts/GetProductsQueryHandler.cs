using Application.Common.Interfaces.Persistence;
using Application.Products.Queries.Common.DTOs;
using Domain.ProductAggregate;
using Domain.ProductAggregate.Enumerations;
using MediatR;

namespace Application.Products.Queries.GetProducts;

/// <summary>
/// Handles the <see cref="GetProductsQuery"/> query.
/// </summary>
public sealed class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, ProductListResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private const int DefaultProductQuantityToTake = 20;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetProductsQueryHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    public GetProductsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc/>
    public async Task<ProductListResult> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Product> products;
        var limit = request.Limit ?? DefaultProductQuantityToTake;

        if (request.categories != null && request.categories.Any())
        {
            var categoryIds = new HashSet<long>(request.categories.Select(Category.Create).Select(c => c.Id));

            products = await _unitOfWork.ProductRepository.FindAllAsync(product =>
                product.ProductCategories.Any(pc => categoryIds.Contains(pc.CategoryId))
            );
        }
        else
        {
            products = await _unitOfWork.ProductRepository.FindAllAsync();
        }

        return new ProductListResult(products.Take(limit));
    }
}
