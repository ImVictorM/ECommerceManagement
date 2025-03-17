using Domain.OrderAggregate.Services;
using Domain.OrderAggregate.ValueObjects;
using Domain.ProductAggregate.Services;
using Domain.ProductAggregate.ValueObjects;

namespace Application.Orders.Services;

internal class OrderAssemblyService : IOrderAssemblyService
{
    private readonly IInventoryManagementService _inventoryManagementService;
    private readonly IProductPricingService _productPricingService;

    public OrderAssemblyService(
        IInventoryManagementService inventoryManagementService,
        IProductPricingService productPricingService
    )
    {
        _inventoryManagementService = inventoryManagementService;
        _productPricingService = productPricingService;
    }

    public async Task<IEnumerable<OrderLineItem>> AssembleOrderLineItemsAsync(
        IEnumerable<OrderLineItemDraft> lineItemDrafts,
        CancellationToken cancellationToken = default
    )
    {
        var productsReserved = lineItemDrafts.Select(op => ProductReserved.Create(
            op.ProductId,
            op.Quantity
        ));

        var products = await _inventoryManagementService.ReserveInventoryAsync(
            productsReserved,
            cancellationToken
        );

        var productOnSalePrices = await _productPricingService
            .CalculateProductsPriceApplyingSaleAsync(
                products,
                cancellationToken
            );

        var productsMap = products.ToDictionary(p => p.Id);

        return productsReserved.Select(productReserved => OrderLineItem.Create(
            productReserved.ProductId,
            productReserved.QuantityReserved,
            productsMap[productReserved.ProductId].BasePrice,
            productOnSalePrices[productReserved.ProductId],
            productsMap[productReserved.ProductId].ProductCategories
                .Select(c => c.CategoryId)
                .ToHashSet()
        ));
    }
}
