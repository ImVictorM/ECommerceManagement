using Application.Common.Interfaces.Persistence;
using Application.Products.Queries.Common.Errors;
using Domain.ProductAggregate.ValueObjects;
using MediatR;

namespace Application.Products.Commands.UpdateProductDiscounts;

/// <summary>
/// Handles the <see cref="UpdateProductDiscountsCommand"/> command.
/// </summary>
public class UpdateProductDiscountsCommandHandler : IRequestHandler<UpdateProductDiscountsCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="UpdateProductDiscountsCommandHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    public UpdateProductDiscountsCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc/>
    public async Task<Unit> Handle(UpdateProductDiscountsCommand request, CancellationToken cancellationToken)
    {
        var productId = ProductId.Create(request.Id);
        var product =
            await _unitOfWork.ProductRepository.FindByIdAsync(productId)
            ?? throw new ProductNotFoundException($"Product with id {productId} does not exist");

        product.ClearDiscounts();

        product.AddDiscounts(request.Discounts.ToArray());

        await _unitOfWork.SaveChangesAsync();

        return Unit.Value;
    }
}
