using Application.Common.Interfaces.Persistence;
using Application.Products.Queries.Common.Errors;
using Domain.ProductAggregate.Specifications;
using Domain.ProductAggregate.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Products.Commands.UpdateProductDiscounts;

/// <summary>
/// Handles the <see cref="UpdateProductDiscountsCommand"/> command.
/// </summary>
public sealed partial class UpdateProductDiscountsCommandHandler : IRequestHandler<UpdateProductDiscountsCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="UpdateProductDiscountsCommandHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="logger">The logger.</param>
    public UpdateProductDiscountsCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateProductDiscountsCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<Unit> Handle(UpdateProductDiscountsCommand request, CancellationToken cancellationToken)
    {
        LogInitiatingProductDiscountsUpdate(request.Id);

        var productId = ProductId.Create(request.Id);
        var product = await _unitOfWork.ProductRepository.FindFirstSatisfyingAsync(new QueryActiveProductByIdSpecification(productId));

        if (product == null)
        {
            LogProductDoesNotExist();
            throw new ProductNotFoundException($"Product with id {productId} does not exist");
        }

        LogClearingCurrentDiscounts(product.Discounts.Count);
        product.ClearDiscounts();

        var newDiscounts = request.Discounts.ToArray();

        LogAddingNewDiscounts(newDiscounts.Length);
        product.AddDiscounts(newDiscounts);

        await _unitOfWork.SaveChangesAsync();

        LogDiscountsUpdatedSuccessfully();
        return Unit.Value;
    }
}
