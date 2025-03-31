using Domain.ProductAggregate.Services;
using Domain.ProductAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;

using Application.Orders.Services;

using FluentAssertions;
using Bogus;
using Moq;

namespace Application.UnitTests.Orders.Services;

/// <summary>
/// Unit tests for the <see cref="OrderAssemblyService"/> service.
/// </summary>
public class OrderAssemblyServiceTests
{
    private readonly OrderAssemblyService _service;
    private readonly Mock<IInventoryManagementService> _mockInventoryManagementService;
    private readonly Mock<IProductPricingService> _mockProductPricingService;
    private readonly Faker _faker = new();

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderAssemblyServiceTests"/>
    /// class.
    /// </summary>
    public OrderAssemblyServiceTests()
    {
        _mockInventoryManagementService = new Mock<IInventoryManagementService>();
        _mockProductPricingService = new Mock<IProductPricingService>();

        _service = new OrderAssemblyService(
            _mockInventoryManagementService.Object,
            _mockProductPricingService.Object
        );
    }

    /// <summary>
    /// Verifies the order line items assemble occurs correctly.
    /// </summary>
    [Fact]
    public async Task AssembleOrderLineItemsAsync_WithValidDrafts_ReturnsOrderLineItems()
    {
        var lineItemDrafts = new[]
        {
            OrderUtils.CreateOrderLineItemDraft(
                productId: ProductId.Create(1),
                quantity: 2
            ),
            OrderUtils.CreateOrderLineItemDraft(
                productId: ProductId.Create(2),
                quantity: 1
            )
        };

        var productsReserved = lineItemDrafts
            .Select(d => ProductReserved.Create(
                d.ProductId,
                d.Quantity
            ))
            .ToList();

        var products = productsReserved
            .Select(r => ProductUtils.CreateProduct(
                id: r.ProductId,
                initialQuantityInInventory: 200
            ))
            .ToList();

        var productsMap = products.ToDictionary(p => p.Id);

        var productsBasePrice = products.ToDictionary(
            p => p.Id,
            p => p.BasePrice
        );

        var productsCalculatedPrice = products.ToDictionary(
            p => p.Id,
            // simulate price with random discount from 10% to 50%
            p => p.BasePrice * _faker.Random.Decimal(0.5m, 0.9m)
        );

        var expectedLineItems = productsReserved.Select(p =>
            OrderUtils.CreateOrderLineItem(
                productId: p.ProductId,
                quantity: p.QuantityReserved,
                basePrice: productsBasePrice[p.ProductId],
                purchasedPrice: productsCalculatedPrice[p.ProductId],
                productCategories: productsMap[p.ProductId].ProductCategories
                    .Select(c => c.CategoryId)
                    .ToHashSet()
            )
        );

        _mockInventoryManagementService
            .Setup(s => s.ReserveInventoryAsync(
                productsReserved,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(products);

        _mockProductPricingService
            .Setup(s => s.CalculateDiscountedPricesAsync(
                products,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(productsCalculatedPrice);

        var result = await _service.AssembleOrderLineItemsAsync(lineItemDrafts);

        result.Should().HaveCount(2);

        result.Should().BeEquivalentTo(expectedLineItems);
    }
}
