using Application.Common.Persistence;
using Application.Products.Errors;

using Domain.ProductAggregate.Specifications;
using Domain.ProductAggregate.ValueObjects;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Products.Commands.DeactivateProduct;

/// <summary>
/// Handles the command to deactivate a product.
/// </summary>
public sealed partial class DeactivateProductCommandHandler : IRequestHandler<DeactivateProductCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductRepository _productRepository;

    /// <summary>
    /// Initiates a new instance of the <see cref="DeactivateProductCommandHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="productRepository">The product repository.</param>
    /// <param name="logger">The logger.</param>
    public DeactivateProductCommandHandler(
        IUnitOfWork unitOfWork,
        IProductRepository productRepository,
        ILogger<DeactivateProductCommandHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<Unit> Handle(DeactivateProductCommand request, CancellationToken cancellationToken)
    {
        LogInitiatingProductDeactivation(request.Id);

        var productId = ProductId.Create(request.Id);

        var product = await _productRepository.FindFirstSatisfyingAsync(
            new QueryActiveProductByIdSpecification(productId),
            cancellationToken
        );

        if (product == null)
        {
            LogProductToBeDeactivateDoesNotExist();

            throw new ProductNotFoundException($"Product with id {productId} could not be deactivated because it does not exist or is already inactive");
        }

        product.Deactivate();

        await _unitOfWork.SaveChangesAsync();

        LogDeactivationCompleted();

        return Unit.Value;
    }
}
