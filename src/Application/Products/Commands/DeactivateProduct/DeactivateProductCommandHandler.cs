using Application.Common.Interfaces.Persistence;
using Application.Products.Queries.Common.Errors;
using Domain.ProductAggregate.Specifications;
using Domain.ProductAggregate.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Products.Commands.DeactivateProduct;

/// <summary>
/// Handles the command to deactivate a product.
/// </summary>
public sealed partial class DeactivateProductCommandHandler : IRequestHandler<DeactivateProductCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="DeactivateProductCommandHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="logger">The logger.</param>
    public DeactivateProductCommandHandler(IUnitOfWork unitOfWork, ILogger<DeactivateProductCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<Unit> Handle(DeactivateProductCommand request, CancellationToken cancellationToken)
    {
        LogInitiatingProductDeactivation(request.Id);

        var productId = ProductId.Create(request.Id);

        var product = await _unitOfWork.ProductRepository
            .FindByIdSatisfyingAsync(productId, new QueryProductActiveSpec());

        if (product == null)
        {
            LogProductToBeDeactivateDoesNotExist();

            throw new ProductNotFoundException($"Product with id {productId} could not be deactivated because it does not exist or is already inactive");
        }

        product.MakeInactive();

        await _unitOfWork.SaveChangesAsync();

        LogDeactivationCompleted();

        return Unit.Value;
    }
}
