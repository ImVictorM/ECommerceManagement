using Application.Common.Persistence.Repositories;
using Application.Products.Errors;
using Application.Products.Services;

using Domain.ProductAggregate.Specifications;
using Domain.ProductAggregate;
using Domain.UnitTests.TestUtils;

using SharedKernel.Interfaces;

using FluentAssertions;
using Bogus;
using Moq;

namespace Application.UnitTests.Products.Services;

/// <summary>
/// Unit tests for the <see cref="InventoryManagementService"/> service.
/// </summary>
public class InventoryManagementServiceTests
{
    private readonly InventoryManagementService _service;
    private readonly Mock<IProductRepository> _mockProductRepository;
    private static readonly Faker _faker = new();

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="InventoryManagementServiceTests"/> class.
    /// </summary>
    public InventoryManagementServiceTests()
    {
        _mockProductRepository = new Mock<IProductRepository>();
        _service = new InventoryManagementService(
            _mockProductRepository.Object
        );
    }

    /// <summary>
    /// Verifies that inventory is correctly reserved for valid products.
    /// </summary>
    [Fact]
    public async Task ReserveInventoryAsync_WithValidProducts_ReservesStockCorrectly()
    {
        var productsReserved = ProductUtils.CreateProductsReserved(
            count: 3,
            quantityMin: 1,
            quantityMax: 50
        );

        var productIds = productsReserved.Select(p => p.ProductId);
        var products = productIds.Select(id => ProductUtils.CreateProduct(
            id: id,
            initialQuantityInInventory: _faker.Random.Int(1000, 5000)
        ));

        _mockProductRepository
            .Setup(repo => repo.FindSatisfyingAsync(
                It.IsAny<ISpecificationQuery<Product>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(products);

        var result = await _service.ReserveInventoryAsync(productsReserved);

        result.Should().BeEquivalentTo(products);
    }

    /// <summary>
    /// Verifies an exception is thrown when trying to reserve an invalid product.
    /// </summary>
    [Fact]
    public async Task ReserveInventoryAsync_WithInvalidProduct_ThrowsError()
    {
        var productsReserved = ProductUtils.CreateProductsReserved(count: 1);

        var emptyProducts = new List<Product>();

        _mockProductRepository
            .Setup(repo => repo.FindSatisfyingAsync(
                It.IsAny<QueryActiveProductSpecification>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(emptyProducts);

        await FluentActions
            .Invoking(() => _service.ReserveInventoryAsync(productsReserved))
            .Should()
            .ThrowAsync<ProductNotFoundException>();
    }
}
