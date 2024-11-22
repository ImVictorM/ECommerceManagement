using Application.Common.Interfaces.Persistence;
using Application.Products.Queries.Common.DTOs;
using Application.Products.Queries.Common.Errors;
using Domain.ProductAggregate.Specifications;
using Domain.ProductAggregate.ValueObjects;
using MediatR;

namespace Application.Products.Queries.GetProductById;

/// <summary>
/// Query handler for the <see cref="GetProductByIdQuery"/> query;
/// </summary>
public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductResult>
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetProductByIdQueryHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    public GetProductByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc/>
    public async Task<ProductResult> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var productId = ProductId.Create(request.Id);

        var product = await _unitOfWork.ProductRepository
            .FindByIdSatisfyingAsync(productId, new QueryProductActiveSpec())
            ?? throw new ProductNotFoundException($"The product with id {productId} does not exist");

        return new ProductResult(product);
    }
}
