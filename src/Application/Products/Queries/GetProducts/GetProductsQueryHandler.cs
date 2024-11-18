using Application.Common.Interfaces.Persistence;
using Application.Products.Queries.Common.DTOs;
using MediatR;

namespace Application.Products.Queries.GetProducts;

/// <summary>
/// Handles the <see cref="GetProductsQuery"/> query.
/// </summary>
public sealed class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, ProductListResult>
{
    private readonly IUnitOfWork _unitOfWork;

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
        var products = await _unitOfWork.ProductRepository.FindAllAsync();

        return new ProductListResult(products.Take(request.Limit));
    }
}
