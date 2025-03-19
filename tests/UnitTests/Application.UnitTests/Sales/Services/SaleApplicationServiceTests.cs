using Application.Common.Persistence.Repositories;
using Application.Sales.Services;

using Domain.CategoryAggregate.ValueObjects;
using Domain.ProductAggregate.ValueObjects;
using Domain.SaleAggregate;
using Domain.SaleAggregate.ValueObjects;
using Domain.SaleAggregate.Specifications;
using Domain.UnitTests.TestUtils;

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
    public async Task GetApplicableSalesForProductsAsync_WithValidProducts_ReturnsCorrectSalesForEachProduct()
    {
        var products = new[]
        {
            SaleEligibleProduct.Create(
                ProductId.Create(1),
                [
                    CategoryId.Create(1)
                ]
            ),
            SaleEligibleProduct.Create(
                ProductId.Create(2),
                [
                    CategoryId.Create(2)
                ]
            )
        };

        var expectedSales = new Dictionary<ProductId, IEnumerable<Sale>>
        {
            [products[0].ProductId] =
            [
                await SaleUtils.CreateSaleAsync(
                    productsOnSale:
                    [
                        SaleProduct.Create(products[0].ProductId)
                    ],
                    categoriesOnSale: [],
                    productsExcludedFromSale: []
                ),
            ],
            [products[1].ProductId] =
            [
                await SaleUtils.CreateSaleAsync(
                    categoriesOnSale:
                    [
                        SaleCategory.Create(products[1].CategoryIds.First())
                    ],
                    productsExcludedFromSale: [],
                    productsOnSale: []
                ),
            ]
        };

        _mockSaleRepository
            .Setup(r => r.FindSatisfyingAsync(
                It.IsAny<QueryApplicableSalesForProductsSpecification>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(expectedSales.SelectMany(kvp => kvp.Value));

        var result = await _service.GetApplicableSalesForProductsAsync(products);

        result.Should().BeEquivalentTo(expectedSales);
    }
}
