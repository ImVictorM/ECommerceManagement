using Application.Common.Interfaces.Persistence;
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
/// Unit tests for the <see cref="SaleService"/> service.
/// </summary>
public class SaleServiceTests
{
    private readonly SaleService _service;
    private readonly Mock<IRepository<Sale, SaleId>> _mockSaleRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="SaleServiceTests"/> class.
    /// </summary>
    public SaleServiceTests()
    {
        _mockSaleRepository = new Mock<IRepository<Sale, SaleId>>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _mockUnitOfWork.Setup(uow => uow.SaleRepository).Returns(_mockSaleRepository.Object);

        _service = new SaleService(_mockUnitOfWork.Object);
    }

    /// <summary>
    /// Verifies the corresponding sales is returned to a product.
    /// </summary>
    [Fact]
    public async Task GetProductSales_WhenCalled_ReturnsSalesWhereProductIsIncluded()
    {
        var product = SaleProduct.Create(
            productId: ProductId.Create(1),
            new HashSet<CategoryId>()
            {
                CategoryId.Create(1),
                CategoryId.Create(2)
            }
        );

        var expectedSales = new[]
        {
            SaleUtils.CreateSale(
                productsInSale: new HashSet<ProductReference>()
                {
                    ProductReference.Create(ProductId.Create(1))
                }
            ),
            SaleUtils.CreateSale(
                categoriesInSale: new HashSet<CategoryReference>()
                {
                    CategoryReference.Create(CategoryId.Create(2))
                }
            ),
        };

        _mockSaleRepository
            .Setup(r => r.FindAllAsync(It.IsAny<Expression<Func<Sale, bool>>>()))
            .ReturnsAsync(expectedSales);

        var result = await _service.GetProductSalesAsync(product);

        result.Should().BeEquivalentTo(expectedSales);
    }
}
