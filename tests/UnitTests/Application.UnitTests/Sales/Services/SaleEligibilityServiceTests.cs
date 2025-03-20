using Application.Common.Persistence.Repositories;
using Application.Sales.Errors;
using Application.Sales.Services;

using Domain.CategoryAggregate.ValueObjects;
using Domain.ProductAggregate.Specifications;
using Domain.ProductAggregate.ValueObjects;
using Domain.SaleAggregate;
using Domain.SaleAggregate.Services;
using Domain.SaleAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;

using SharedKernel.Interfaces;
using SharedKernel.UnitTests.TestUtils;
using SharedKernel.ValueObjects;

using FluentAssertions;
using Moq;

namespace Application.UnitTests.Sales.Services;

/// <summary>
/// Unit tests for the <see cref="SaleEligibilityService"/> service.
/// </summary>
public class SaleEligibilityServiceTests
{
    private readonly SaleEligibilityService _service;
    private readonly Mock<ISaleApplicationService> _mockSaleApplicationService;
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly Mock<IDiscountService> _mockDiscountService;

    /// <summary>
    /// Initializes a new instance of the <see cref="SaleEligibilityServiceTests"/> class.
    /// </summary>
    public SaleEligibilityServiceTests()
    {
        _mockSaleApplicationService = new Mock<ISaleApplicationService>();
        _mockProductRepository = new Mock<IProductRepository>();
        _mockDiscountService = new Mock<IDiscountService>();

        _service = new SaleEligibilityService(
            _mockSaleApplicationService.Object,
            _mockProductRepository.Object,
            _mockDiscountService.Object
        );
    }

    /// <summary>
    /// Verifies that a sale is eligible when all products satisfy the discount
    /// threshold.
    /// </summary>
    [Fact]
    public async Task IsSaleEligibleAsync_WhenAllProductsSatisfyThreshold_ReturnsTrue()
    {
        var product = ProductUtils.CreateProduct(
            id: ProductId.Create(1),
            basePrice: 100m,
            categories: [ProductCategory.Create(CategoryId.Create(2))]
        );

        var sale = SaleUtils.CreateSale(
            discount: DiscountUtils.CreateDiscount(
                percentage: PercentageUtils.Create(10)
            ),
            productsOnSale:
            [
                SaleProduct.Create(product.Id)
            ]
        );

        var productOnSalePrice = 90m;

        _mockProductRepository
            .Setup(r => r.FindSatisfyingAsync(
                It.IsAny<QueryProductsContainingIdsSpecification>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync([product]);

        var applicableSales = new Dictionary<ProductId, IEnumerable<Sale>>
        {
            [product.Id] = []
        };

        _mockSaleApplicationService
            .Setup(s => s.GetApplicableSalesForProductsAsync(
                It.IsAny<IEnumerable<SaleEligibleProduct>>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(applicableSales);

        _mockDiscountService
            .Setup(d => d.CalculateDiscountedPrice(
                It.IsAny<decimal>(),
                It.IsAny<IEnumerable<Discount>>()
            ))
            .Returns(productOnSalePrice);

        await FluentActions
            .Invoking(() => _service.EnsureSaleProductsEligibilityAsync(sale, default))
            .Should()
            .NotThrowAsync();
    }

    /// <summary>
    /// Verifies that a sale is not eligible when at least one product does not
    /// satisfy the discount threshold.
    /// </summary>
    [Fact]
    public async Task IsSaleEligibleAsync_WhenAnyProductFailsThreshold_ReturnsFalse()
    {
        var product = ProductUtils.CreateProduct(
            id: ProductId.Create(1),
            basePrice: 100m,
            categories: [ProductCategory.Create(CategoryId.Create(2))]
        );

        var sale = SaleUtils.CreateSale(
            discount: DiscountUtils.CreateDiscount(
                percentage: PercentageUtils.Create(91)
            ),
            productsOnSale:
            [
                SaleProduct.Create(ProductId.Create(1))
            ]
        );

        var productOnSalePrice = 9m;

        _mockProductRepository
            .Setup(r => r.FindSatisfyingAsync(
                It.IsAny<QueryProductsContainingIdsSpecification>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync([product]);

        var applicableSales = new Dictionary<ProductId, IEnumerable<Sale>>
        {
            [product.Id] = []
        };

        _mockSaleApplicationService
            .Setup(s => s.GetApplicableSalesForProductsAsync(
                It.IsAny<IEnumerable<SaleEligibleProduct>>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(applicableSales);

        _mockDiscountService
            .Setup(d => d.CalculateDiscountedPrice(
                It.IsAny<decimal>(),
                It.IsAny<IEnumerable<Discount>>()
            ))
            .Returns(productOnSalePrice);

        await FluentActions
            .Invoking(() => _service.EnsureSaleProductsEligibilityAsync(sale, default))
            .Should()
            .ThrowAsync<SaleProductNotEligibleException>();
    }
}
