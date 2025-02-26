using Application.Common.Persistence.Repositories;
using Application.Products.DTOs;

using Domain.ProductAggregate;
using Domain.ProductAggregate.ValueObjects;

using SharedKernel.Interfaces;

using Infrastructure.Common.Persistence;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Products;

internal sealed class ProductRepository : BaseRepository<Product, ProductId>, IProductRepository
{
    public ProductRepository(IECommerceDbContext dbContext) : base(dbContext)
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
            .Where(specification.Criteria)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(product => new ProductWithCategoriesQueryResult(
                product,
                Context.Categories
                    .Where(category => product.ProductCategories.Select(pc => pc.CategoryId).Contains(category.Id))
                    .Select(category => category.Name)
                    .ToList()
            ))
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<ProductWithCategoriesQueryResult?> GetProductWithCategoriesSatisfyingAsync(
        ISpecificationQuery<Product> specification,
        CancellationToken cancellationToken = default
    )
    {
        return await Context.Products
            .Where(specification.Criteria)
            .Select(product => new ProductWithCategoriesQueryResult(
                product,
                Context.Categories
                    .Where(category => product.ProductCategories.Select(pc => pc.CategoryId).Contains(category.Id))
                    .Select(category => category.Name)
                    .ToList()
            ))
            .FirstOrDefaultAsync(cancellationToken);
    }
}
