using Application.Common.DTOs.Pagination;
using Application.Common.Persistence.Repositories;

using Domain.ProductAggregate;
using Domain.ProductAggregate.ValueObjects;

using SharedKernel.Interfaces;

using Infrastructure.Common.Persistence;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Products;

internal sealed class ProductRepository
    : BaseRepository<Product, ProductId>, IProductRepository
{
    public ProductRepository(IECommerceDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IReadOnlyList<Product>> GetProductsSatisfyingAsync(
        ISpecificationQuery<Product> specification,
        PaginationParams paginationParams,
        CancellationToken cancellationToken = default
    )
    {
        var page = paginationParams.Page;
        var pageSize = paginationParams.PageSize;

        return await Context.Products
            .Where(specification.Criteria)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }
}
