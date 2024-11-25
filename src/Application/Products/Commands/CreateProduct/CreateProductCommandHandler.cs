using Application.Common.DTOs;
using Application.Common.Interfaces.Persistence;
using Domain.ProductAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Products.Commands.CreateProduct;

/// <summary>
/// Handles product creation.
/// </summary>
public sealed partial class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, CreatedResult>
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="CreateProductCommandHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unity of work.</param>
    /// <param name="logger">The logger.</param>
    public CreateProductCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateProductCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<CreatedResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        LogCreatingProduct(request.Name);

        var newProduct = Product.Create(
            request.Name,
            request.Description,
            request.BasePrice,
            request.InitialQuantity,
            request.Categories,
            request.Images,
            request.InitialDiscounts
        );

        LogProductCreatedInitiatingPersistence();

        await _unitOfWork.ProductRepository.AddAsync(newProduct);

        await _unitOfWork.SaveChangesAsync();

        LogProductPersistedSuccessfully(newProduct.Id.ToString());

        return new CreatedResult(newProduct.Id.ToString());
    }
}
