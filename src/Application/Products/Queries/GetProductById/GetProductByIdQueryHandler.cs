using Application.Common.Interfaces.Persistence;
using Application.Products.Queries.Common.DTOs;
using Application.Products.Queries.Common.Errors;
using Domain.ProductAggregate.Specifications;
using Domain.ProductAggregate.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Products.Queries.GetProductById;

/// <summary>
/// Query handler for the <see cref="GetProductByIdQuery"/> query;
/// </summary>
public sealed partial class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductResult>
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetProductByIdQueryHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="logger">The logger.</param>
    public GetProductByIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetProductByIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<ProductResult> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        LogInitiateRetrievingProductById(request.Id);

        var productId = ProductId.Create(request.Id);

        var product = await _unitOfWork.ProductRepository.FindByIdSatisfyingAsync(productId, new QueryProductActiveSpec());

        if (product == null)
        {
            LogProductDoesNotExist();
            throw new ProductNotFoundException($"The product with id {productId} does not exist");
        }

        LogProductFound();
        return new ProductResult(product);
    }
}
