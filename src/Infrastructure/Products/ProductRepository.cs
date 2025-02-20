using Application.Common.Persistence;
using Application.Products.DTOs;

using Domain.ProductAggregate;
using Domain.ProductAggregate.ValueObjects;

using SharedKernel.Interfaces;

using Infrastructure.Common.Persistence;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Products;

/// <summary>
/// Defines the implementation for product persistence operations.
/// </summary>
public sealed class ProductRepository : BaseRepository<Product, ProductId>, IProductRepository
{
    /// <summary>
    /// Initiates a new instance of the <see cref="ProductRepository"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public ProductRepository(ECommerceDbContext dbContext) : base(dbContext)
    {
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ProductWithCategoriesQueryResult>> GetProductsWithCategoriesSatisfyingAsync(
        ISpecificationQuery<Product> specification,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default
    )
    {
        return await Context.Products
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Where(specification.Criteria)
            .Select(product => new ProductWithCategoriesQueryResult(
                product,
                GetProductCategoryNames(product.ProductCategories)
            ))
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<ProductWithCategoriesQueryResult> GetProductWithCategoriesSatisfyingAsync(
        ISpecificationQuery<Product> specification,
        CancellationToken cancellationToken = default
    )
    {
        return await Context.Products
            .Where(specification.Criteria)
            .Select(product => new ProductWithCategoriesQueryResult(
                product,
                GetProductCategoryNames(product.ProductCategories)
            ))
            .FirstAsync(cancellationToken);
    }

    private IEnumerable<string> GetProductCategoryNames(IEnumerable<ProductCategory> productCategories)
    {
        return Context.Categories
            .Where(category => productCategories.Select(pc => pc.CategoryId).Contains(category.Id))
            .Select(category => category.Name);
    }
}
