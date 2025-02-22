using Application.Products.DTOs;

using Domain.ProductAggregate;
using Domain.ProductAggregate.ValueObjects;

using SharedKernel.Interfaces;

namespace Application.Common.Persistence;

/// <summary>
/// Defines the contract for product persistence operations.
/// </summary>
public interface IProductRepository : IBaseRepository<Product, ProductId>
{
    /// <summary>
    /// Retrieves the products satisfying a specification including the product category names.
    /// Supports pagination.
    /// </summary>
    /// <param name="specification">The specification.</param>
    /// <param name="page">The current page.</param>
    /// <param name="pageSize">The page size.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A paginated list of filtered products with the category names.</returns>
    Task<IEnumerable<ProductWithCategoriesQueryResult>> GetProductsWithCategoriesSatisfyingAsync(
        ISpecificationQuery<Product> specification,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Retrieves a product satisfying a specification including the product category names.
    /// </summary>
    /// <param name="specification">The specification.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The product with the category names.</returns>
    Task<ProductWithCategoriesQueryResult?> GetProductWithCategoriesSatisfyingAsync(
        ISpecificationQuery<Product> specification,
        CancellationToken cancellationToken = default
    );
}
