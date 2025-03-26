using Application.Common.Persistence.Repositories;
using Application.Common.Persistence;
using Application.Products.Errors;

using Domain.ProductAggregate.Specifications;
using Domain.ProductAggregate.ValueObjects;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Products.Commands.DeactivateProduct;

internal sealed partial class DeactivateProductCommandHandler
    : IRequestHandler<DeactivateProductCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductRepository _productRepository;

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

    public async Task<Unit> Handle(
        DeactivateProductCommand request,
        CancellationToken cancellationToken
    )
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

            throw new ProductNotFoundException(
                "The product to be deactivated either does not exist" +
                " or is already inactive"
            )
            .WithContext("ProductId", productId.ToString());
        }

        product.Deactivate();

        await _unitOfWork.SaveChangesAsync();

        LogDeactivationCompleted();

        return Unit.Value;
    }
}
