using System.Globalization;
using Application.Common.DTOs;
using Application.Common.Interfaces.Persistence;
using Domain.ProductAggregate;
using MediatR;

namespace Application.Products.Commands.CreateProduct;

/// <summary>
/// Handles product creation.
/// </summary>
public sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, CreatedResult>
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="CreateProductCommandHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unity of work.</param>
    public CreateProductCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc/>
    public async Task<CreatedResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var newProduct = Product.Create(
            request.Name,
            request.Description,
            request.InitialPrice,
            request.InitialQuantity,
            request.Categories,
            request.Images,
            request.InitialDiscounts
        );

        await _unitOfWork.ProductRepository.AddAsync(newProduct);

        var createdId = await _unitOfWork.SaveChangesAsync();

        return new CreatedResult(createdId.ToString(CultureInfo.InvariantCulture));
    }
}
