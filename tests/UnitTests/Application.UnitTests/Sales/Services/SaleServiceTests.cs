using Application.Common.Persistence;
using Application.Sales.Services;

using Domain.CategoryAggregate.ValueObjects;
using Domain.ProductAggregate.ValueObjects;
using Domain.SaleAggregate;
using Domain.SaleAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;

using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using SharedKernel.UnitTests.TestUtils;

namespace Application.UnitTests.Sales.Services;

/// <summary>
/// Unit tests for the <see cref="SaleService"/> service.
/// </summary>
public class SaleServiceTests
{
    private readonly SaleService _service;
    private readonly Mock<ISaleRepository> _mockSaleRepository;

    /// <summary>
    /// Initiates a new instance of the <see cref="SaleServiceTests"/> class.
    /// </summary>
    public SaleServiceTests()
    {
        _mockSaleRepository = new Mock<ISaleRepository>();

        _service = new SaleService(_mockSaleRepository.Object);
    }

    /// <summary>
    /// Verifies that the method returns the correct sales for multiple products.
    /// </summary>
    [Fact]
    public async Task GetProductsSalesAsync_WhenCalled_ReturnsCorrectSalesForEachProduct()
    {
        var products = new[]
        {
            SaleProduct.Create(ProductId.Create(1), new HashSet<CategoryId> { CategoryId.Create(1) }),
            SaleProduct.Create(ProductId.Create(2), new HashSet<CategoryId> { CategoryId.Create(2) })
        };

        var expectedSales = new Dictionary<ProductId, IEnumerable<Sale>>
        {
            [products[0].ProductId] =
            [
                SaleUtils.CreateSale(
                    discount: DiscountUtils.CreateDiscount(
                        percentage: PercentageUtils.Create(10),
                        startingDate: DateTimeOffset.UtcNow.AddHours(-5),
                        endingDate: DateTimeOffset.UtcNow.AddHours(5)
                    ),
                    productsInSale: new HashSet<ProductReference>
                    {
                        ProductReference.Create(products[0].ProductId)
                    },
                    categoriesInSale: new HashSet<CategoryReference>(),
                    productsExcludeFromSale: new HashSet<ProductReference>()
                ),
            ],
            [products[1].ProductId] =
            [
                SaleUtils.CreateSale(
                    discount: DiscountUtils.CreateDiscount(
                        percentage: PercentageUtils.Create(5),
                        startingDate: DateTimeOffset.UtcNow.AddHours(-5),
                        endingDate: DateTimeOffset.UtcNow.AddHours(5)
                    ),
                    categoriesInSale: new HashSet<CategoryReference>
                    {
                        CategoryReference.Create(CategoryId.Create(2))
                    },
                    productsExcludeFromSale: new HashSet<ProductReference>(),
                    productsInSale: new HashSet<ProductReference>()
                ),
            ]
        };

        _mockSaleRepository
            .Setup(r => r.FindAllAsync(
                It.IsAny<Expression<Func<Sale, bool>>>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(expectedSales.SelectMany(kvp => kvp.Value));

        var result = await _service.GetProductsSalesAsync(products);

        result.Should().BeEquivalentTo(expectedSales);
    }
}
