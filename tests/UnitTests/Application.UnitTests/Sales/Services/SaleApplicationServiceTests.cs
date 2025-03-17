using Application.Common.Persistence.Repositories;
using Application.Sales.Services;

using Domain.CategoryAggregate.ValueObjects;
using Domain.ProductAggregate.ValueObjects;
using Domain.SaleAggregate;
using Domain.SaleAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;

using System.Linq.Expressions;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Sales.Services;

/// <summary>
/// Unit tests for the <see cref="SaleApplicationService"/> service.
/// </summary>
public class SaleApplicationServiceTests
{
    private readonly SaleApplicationService _service;
    private readonly Mock<ISaleRepository> _mockSaleRepository;

    /// <summary>
    /// Initiates a new instance of the <see cref="SaleApplicationServiceTests"/>
    /// class.
    /// </summary>
    public SaleApplicationServiceTests()
    {
        _mockSaleRepository = new Mock<ISaleRepository>();

        _service = new SaleApplicationService(_mockSaleRepository.Object);
    }

    /// <summary>
    /// Verifies that the method returns the correct sales for multiple products.
    /// </summary>
    [Fact]
    public async Task GetApplicableSalesForProductsAsync_WhenCalled_ReturnsCorrectSalesForEachProduct()
    {
        var products = new[]
        {
            SaleProduct.Create(
                ProductId.Create(1),
                new HashSet<CategoryId>
                {
                    CategoryId.Create(1)
                }
            ),
            SaleProduct.Create(
                ProductId.Create(2),
                new HashSet<CategoryId>
                {
                    CategoryId.Create(2)
                }
            )
        };

        var expectedSales = new Dictionary<ProductId, IEnumerable<Sale>>
        {
            [products[0].ProductId] =
            [
                SaleUtils.CreateSale(
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
                    categoriesInSale: new HashSet<CategoryReference>
                    {
                        CategoryReference.Create(products[1].Categories.First())
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

        var result = await _service.GetApplicableSalesForProductsAsync(products);

        result.Should().BeEquivalentTo(expectedSales);
    }
}
