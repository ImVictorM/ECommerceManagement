using Application.Common.Persistence.Repositories;
using Application.Products.Errors;

using Domain.ProductAggregate;
using Domain.ProductAggregate.Services;
using Domain.ProductAggregate.Specifications;
using Domain.ProductAggregate.ValueObjects;

namespace Application.Products.Services;

internal sealed class InventoryManagementService : IInventoryManagementService
{
    private readonly IProductRepository _productRepository;

    public InventoryManagementService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<Product>> ReserveInventoryAsync(
        IEnumerable<ProductReserved> productsReserved,
        CancellationToken cancellationToken = default
    )
    {
        var productIds = productsReserved.Select(p => p.ProductId);

        var specifications =
            new QueryActiveProductSpecification()
            .And(new QueryProductsContainingIdsSpecification(productIds));

        var products = await _productRepository.FindSatisfyingAsync(
            specifications,
            cancellationToken
        );

        var productsMap = products.ToDictionary(p => p.Id);

        foreach (var productReserved in productsReserved)
        {
            if (!productsMap.TryGetValue(
                productReserved.ProductId,
                out var currentProduct
            ))
            {
                throw new ProductNotFoundException(
                    $"The product with id {productReserved.ProductId} could not be" +
                    $" reserved because it was not found"
                );
            }

            currentProduct.Inventory.RemoveStock(productReserved.QuantityReserved);
        }

        return products;
    }
}
