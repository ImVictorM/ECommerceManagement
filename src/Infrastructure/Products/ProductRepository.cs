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
        var query = DbSet
            .AsQueryable()
            .Where(specification.Criteria);

        var page = paginationParams.Page;
        var pageSize = paginationParams.PageSize;

        if (page.HasValue && pageSize.HasValue)
        {
            query = query
                .Skip((page.Value - 1) * pageSize.Value)
                .Take(pageSize.Value);
        }

        return await query.ToListAsync(cancellationToken);
    }
}
