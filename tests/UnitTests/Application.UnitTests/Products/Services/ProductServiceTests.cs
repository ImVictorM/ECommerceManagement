using Application.Products.Services;

using Domain.ProductAggregate.ValueObjects;
using Domain.SaleAggregate.Services;
using Domain.SaleAggregate.ValueObjects;
using Domain.SaleAggregate;
using Domain.UnitTests.TestUtils;

using SharedKernel.UnitTests.TestUtils;
using SharedKernel.Interfaces;
using SharedKernel.ValueObjects;

using FluentAssertions;
using Moq;

namespace Application.UnitTests.Products.Services;

/// <summary>
/// Unit tests for the <see cref="ProductService"/> service.
/// </summary>
public class ProductServiceTests
{
    private readonly ProductService _service;
    private readonly Mock<ISaleService> _mockSaleService;
    private readonly Mock<IDiscountService> _mockDiscountService;

    /// <summary>
    /// Initiates a new instance of the <see cref="ProductServiceTests"/> class.
    /// </summary>
    public ProductServiceTests()
    {
        _mockSaleService = new Mock<ISaleService>();
        _mockDiscountService = new Mock<IDiscountService>();

        _service = new ProductService(
            _mockSaleService.Object,
            _mockDiscountService.Object
        );
    }

    /// <summary>
    /// Tests that <see cref="ProductService.CalculateProductsPriceApplyingSaleAsync"/> correctly applies sales discounts when applicable.
    /// </summary>
    [Fact]
    public async Task CalculateProductsPriceApplyingSaleAsync_WhenSaleIsApplicable_ReturnsCorrectPrice()
    {
        var products = new[]
        {
            ProductUtils.CreateProduct(id: ProductId.Create(1), basePrice: 100),
            ProductUtils.CreateProduct(id: ProductId.Create(2), basePrice: 200)
        };

        var productSales = new Dictionary<ProductId, IEnumerable<Sale>>()
        {
            [products[0].Id] = [SaleUtils.CreateSale(
                discount: DiscountUtils.CreateDiscount(
                    percentage: PercentageUtils.Create(10),
                    startingDate: DateTimeOffset.UtcNow.AddHours(-5),
                    endingDate: DateTimeOffset.UtcNow.AddHours(5)
                ),
                productsInSale: new HashSet<ProductReference> { ProductReference.Create(products[0].Id) },
                categoriesInSale: new HashSet<CategoryReference>(),
                productsExcludeFromSale: new HashSet<ProductReference>()
            )],
            [products[1].Id] = [SaleUtils.CreateSale(
                discount: DiscountUtils.CreateDiscount(
                    percentage: PercentageUtils.Create(20),
                    startingDate: DateTimeOffset.UtcNow.AddHours(-5),
                    endingDate: DateTimeOffset.UtcNow.AddHours(5)
                ),
                productsInSale: new HashSet<ProductReference> { ProductReference.Create(products[1].Id) },
                categoriesInSale: new HashSet<CategoryReference>(),
                productsExcludeFromSale: new HashSet<ProductReference>()
            )]
        };

        var expectedPrices = new Dictionary<ProductId, decimal>
        {
            [products[0].Id] = 100 - (100 * 10 / 100),
            [products[1].Id] = 200 - (200 * 20 / 100)
        };

        _mockSaleService
            .Setup(s => s.GetProductsSalesAsync(
                It.IsAny<IEnumerable<SaleProduct>>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(productSales);

        _mockDiscountService
            .SetupSequence(s => s.CalculateDiscountedPrice(
                It.IsAny<decimal>(),
                It.IsAny<IEnumerable<Discount>>()
            ))
            .Returns(expectedPrices[products[0].Id])
            .Returns(expectedPrices[products[1].Id]);

        var results = await _service.CalculateProductsPriceApplyingSaleAsync(products, default);

        foreach (var result in results)
        {
            expectedPrices.Should().ContainKey(result.Key);
            expectedPrices[result.Key].Should().Be(result.Value);
        }
    }

    /// <summary>
    /// Tests that <see cref="ProductService.CalculateProductsPriceApplyingSaleAsync"/> returns the original price when no sale applies.
    /// </summary>
    [Fact]
    public async Task CalculateProductsPriceApplyingSaleAsync_WhenNoSaleApplies_ReturnsOriginalPrice()
    {
        var product = ProductUtils.CreateProduct(id: ProductId.Create(3), basePrice: 150);

        _mockSaleService
            .Setup(s => s.GetProductsSalesAsync(
                It.IsAny<IEnumerable<SaleProduct>>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(new Dictionary<ProductId, IEnumerable<Sale>>()
            {
                [product.Id] = []
            });

        _mockDiscountService
            .Setup(s => s.CalculateDiscountedPrice(
                It.IsAny<decimal>(),
                It.IsAny<IEnumerable<Discount>>()
            ))
            .Returns(product.BasePrice);

        var results = await _service.CalculateProductsPriceApplyingSaleAsync([product], default);

        results.Should().ContainKey(product.Id);
        results[product.Id].Should().Be(product.BasePrice);
    }

    /// <summary>
    /// Tests that <see cref="ProductService.CalculateProductPriceApplyingSaleAsync"/> applies a discount correctly when a sale is available.
    /// </summary>
    [Fact]
    public async Task CalculateProductPriceApplyingSaleAsync_WhenSaleIsApplicable_ReturnsDiscountedPrice()
    {
        var product = ProductUtils.CreateProduct(id: ProductId.Create(4), basePrice: 300);

        var sale = SaleUtils.CreateSale(
            discount: DiscountUtils.CreateDiscount(
                percentage: PercentageUtils.Create(15),
                startingDate: DateTimeOffset.UtcNow.AddHours(-5),
                endingDate: DateTimeOffset.UtcNow.AddHours(5)
            ),
            productsInSale: new HashSet<ProductReference> { ProductReference.Create(product.Id) },
            categoriesInSale: new HashSet<CategoryReference>(),
            productsExcludeFromSale: new HashSet<ProductReference>()
        );

        var productSales = new Dictionary<ProductId, IEnumerable<Sale>>
        {
            [product.Id] = [sale]
        };

        var expectedPrice = 300 - (300 * 15 / 100);

        _mockSaleService
            .Setup(s => s.GetProductsSalesAsync(
                It.IsAny<IEnumerable<SaleProduct>>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(productSales);

        _mockDiscountService
            .Setup(s => s.CalculateDiscountedPrice(
                It.IsAny<decimal>(),
                It.IsAny<IEnumerable<Discount>>()
            ))
            .Returns(expectedPrice);

        var result = await _service.CalculateProductPriceApplyingSaleAsync(product, default);

        result.Should().Be(expectedPrice);
    }

    /// <summary>
    /// Tests that <see cref="ProductService.CalculateProductPriceApplyingSaleAsync"/> returns the original price when no sale applies.
    /// </summary>
    [Fact]
    public async Task CalculateProductPriceApplyingSaleAsync_WhenNoSaleIsApplicable_ReturnsOriginalPrice()
    {
        var product = ProductUtils.CreateProduct(id: ProductId.Create(5), basePrice: 500);

        _mockSaleService
            .Setup(s => s.GetProductsSalesAsync(
                It.IsAny<IEnumerable<SaleProduct>>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(new Dictionary<ProductId, IEnumerable<Sale>>()
            {
                [product.Id] = []
            });

        _mockDiscountService
            .Setup(s => s.CalculateDiscountedPrice(
                It.IsAny<decimal>(),
                It.IsAny<IEnumerable<Discount>>()
            ))
            .Returns(product.BasePrice);

        var result = await _service.CalculateProductPriceApplyingSaleAsync(product, default);

        result.Should().Be(product.BasePrice);
    }
}
