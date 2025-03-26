using Application.Common.DTOs.Pagination;

using Domain.ProductAggregate;
using Domain.ProductAggregate.ValueObjects;

using SharedKernel.Interfaces;

namespace Application.Common.Persistence.Repositories;

/// <summary>
/// Defines the contract for product persistence operations.
/// </summary>
public interface IProductRepository : IBaseRepository<Product, ProductId>
{
    /// <summary>
    /// Retrieves the products satisfying a specification. Supports pagination.
    /// </summary>
    /// <param name="specification">The specification.</param>
    /// <param name="paginationParams">The pagination parameters.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// A list of <see cref="Product"/>.
    /// </returns>
    Task<IReadOnlyList<Product>> GetProductsSatisfyingAsync(
        ISpecificationQuery<Product> specification,
        PaginationParams paginationParams,
        CancellationToken cancellationToken = default
    );
}
