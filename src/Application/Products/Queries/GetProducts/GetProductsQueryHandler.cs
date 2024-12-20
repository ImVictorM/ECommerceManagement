using Application.Common.Interfaces.Persistence;
using Application.Products.Queries.Common.DTOs;
using Domain.ProductAggregate;
using Domain.ProductAggregate.Specifications;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Models;

namespace Application.Products.Queries.GetProducts;

/// <summary>
/// Handles the <see cref="GetProductsQuery"/> query.
/// </summary>
public sealed partial class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, ProductListResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private const int DefaultProductQuantityToTake = 20;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetProductsQueryHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="logger">The logger.</param>
    public GetProductsQueryHandler(IUnitOfWork unitOfWork, ILogger<GetProductsQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<ProductListResult> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        LogInitiatedRetrievingProducts();
        var limit = request.Limit ?? DefaultProductQuantityToTake;

        CompositeQuerySpecification<Product> spec = new QueryActiveProductsSpecification();

        if (request.Categories != null && request.Categories.Any())
        {
            // TODO: fix this
            //spec.And(new QueryProductByCategorySpecification(Category.Parse(request.Categories)));
        }

        var products = await _unitOfWork.ProductRepository.FindSatisfyingAsync(spec, limit: limit);

        LogProductsRetrievedSuccessfully(limit, request.Categories != null ? string.Join(',', request.Categories) : "none");

        return new ProductListResult(products);
    }
}
