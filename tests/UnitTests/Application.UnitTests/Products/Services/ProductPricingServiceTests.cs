using Application.Products.Services;

using Domain.ProductAggregate.ValueObjects;
using Domain.SaleAggregate.Services;
using Domain.SaleAggregate.ValueObjects;
using Domain.SaleAggregate;
using Domain.UnitTests.TestUtils;

using SharedKernel.Interfaces;
using SharedKernel.ValueObjects;

using FluentAssertions;
using Moq;

namespace Application.UnitTests.Products.Services;

/// <summary>
/// Unit tests for the <see cref="ProductPricingService"/> service.
/// </summary>
public class ProductPricingServiceTests
{
    private readonly ProductPricingService _service;
    private readonly Mock<ISaleApplicationService> _mockSaleService;
    private readonly Mock<IDiscountService> _mockDiscountService;

    /// <summary>
    /// Initiates a new instance of the <see cref="ProductPricingServiceTests"/>
    /// class.
    /// </summary>
    public ProductPricingServiceTests()
    {
        _mockSaleService = new Mock<ISaleApplicationService>();
        _mockDiscountService = new Mock<IDiscountService>();

        _service = new ProductPricingService(
            _mockSaleService.Object,
            _mockDiscountService.Object
        );
    }

    /// <summary>
    /// Verifies the price is calculated for each product when sales is applicable.
    /// </summary>
    [Fact]
    public async Task CalculateProductsPriceApplyingSale_WithApplicableSales_ReturnsCorrectPrice()
    {
        var products = new[]
        {
            ProductUtils.CreateProduct(
                id: ProductId.Create(1),
                basePrice: 100
            ),
            ProductUtils.CreateProduct(
                id: ProductId.Create(2),
                basePrice: 200
            )
        };

        var productSales = products
            .ToDictionary(
                p => p.Id,
                p => new List<Sale>()
                {
                    SaleUtils.CreateSale(
                        productsOnSale: new HashSet<SaleProduct>()
                        {
                            SaleProduct.Create(p.Id)
                        },
                        categoriesInSale: new HashSet<SaleCategory>(),
                        productsExcludedFromSale: new HashSet<SaleProduct>()
                    )
                }
                .AsEnumerable()
            );

        var expectedPrices = products.ToDictionary(
            p => p.Id,
            p => productSales[p.Id]
                .Select(s => s.Discount)
                .OrderByDescending(d => d.Percentage)
                .Aggregate(
                    p.BasePrice,
                    (total, discount) => CalculateExpectedPrice(
                        total,
                        discount.Percentage.Value
                    )
                )
        );

        _mockSaleService
            .Setup(s => s.GetApplicableSalesForProductsAsync(
                It.IsAny<IEnumerable<SaleEligibleProduct>>(),
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

        var results = await _service.CalculateProductsPriceApplyingSaleAsync(
            products,
            default
        );

        foreach (var result in results)
        {
            expectedPrices.Should().ContainKey(result.Key);
            expectedPrices[result.Key].Should().Be(result.Value);
        }
    }

    /// <summary>
    /// Verifies the base price is returned when no sales apply to the products.
    /// </summary>
    [Fact]
    public async Task CalculateProductsPriceApplyingSale_WithNoApplicableSales_ReturnsBasePrice()
    {
        var products = new[]
        {
            ProductUtils.CreateProduct(
                id: ProductId.Create(1),
                basePrice: 100
            ),
            ProductUtils.CreateProduct(
                id: ProductId.Create(2),
                basePrice: 200
            )
        };

        var productSales = products.ToDictionary(
            p => p.Id,
            p => Enumerable.Empty<Sale>()
        );

        _mockSaleService
            .Setup(s => s.GetApplicableSalesForProductsAsync(
                It.IsAny<IEnumerable<SaleEligibleProduct>>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(productSales);

        _mockDiscountService
            .Setup(s => s.CalculateDiscountedPrice(
                It.IsAny<decimal>(),
                It.IsAny<IEnumerable<Discount>>()
            ))
            .Returns((decimal basePrice, IEnumerable<Discount> _) => basePrice);

        var results = await _service.CalculateProductsPriceApplyingSaleAsync(
            products,
            default
        );

        foreach (var product in products)
        {
            results[product.Id].Should().Be(product.BasePrice);
        }
    }

    /// <summary>
    /// Verifies the price is correctly calculated for a single product.
    /// </summary>
    [Fact]
    public async Task CalculateProductPriceApplyingSale_WithApplicableSales_ReturnsCorrectPrice()
    {
        var product = ProductUtils.CreateProduct(
            id: ProductId.Create(1),
            basePrice: 100
        );

        var sale = SaleUtils.CreateSale();

        var productSales = new Dictionary<ProductId, IEnumerable<Sale>>
        {
            [product.Id] = [sale]
        };

        var expectedPrice = CalculateExpectedPrice(
            product.BasePrice,
            sale.Discount.Percentage.Value
        );

        _mockSaleService
            .Setup(s => s.GetApplicableSalesForProductsAsync(
                It.IsAny<IEnumerable<SaleEligibleProduct>>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(productSales);

        _mockDiscountService
            .Setup(s => s.CalculateDiscountedPrice(
                product.BasePrice,
                It.IsAny<IEnumerable<Discount>>()
            ))
            .Returns(expectedPrice);

        var result = await _service.CalculateProductPriceApplyingSaleAsync(
            product,
            default
        );

        result.Should().Be(expectedPrice);
    }

    private static decimal CalculateExpectedPrice(decimal price, int discountPercentage)
    {
        return price - (price * (discountPercentage / 100m));
    }
}
