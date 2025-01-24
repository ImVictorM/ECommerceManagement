using Application.Products.Services;

using Domain.CategoryAggregate;
using Domain.CategoryAggregate.ValueObjects;
using Domain.ProductAggregate;
using Domain.ProductAggregate.ValueObjects;
using Domain.SaleAggregate.Services;
using Domain.SaleAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;

using SharedKernel.UnitTests.TestUtils;

using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using Application.Common.Persistence;

namespace Application.UnitTests.Products.Services;

/// <summary>
/// Unit tests for the <see cref="ProductService"/> service.
/// </summary>
public class ProductServiceTests
{
    private readonly ProductService _service;
    private readonly Mock<ISaleService> _mockSaleService;
    private readonly Mock<IRepository<Product, ProductId>> _mockProductRepository;
    private readonly Mock<IRepository<Category, CategoryId>> _mockCategoryRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="ProductServiceTests"/> class.
    /// </summary>
    public ProductServiceTests()
    {
        _mockSaleService = new Mock<ISaleService>();
        _mockProductRepository = new Mock<IRepository<Product, ProductId>>();
        _mockCategoryRepository = new Mock<IRepository<Category, CategoryId>>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _mockUnitOfWork.Setup(uow => uow.ProductRepository).Returns(_mockProductRepository.Object);
        _mockUnitOfWork.Setup(uow => uow.CategoryRepository).Returns(_mockCategoryRepository.Object);

        _service = new ProductService(_mockUnitOfWork.Object, _mockSaleService.Object);
    }

    /// <summary>
    /// Verifies the product category names is returned.
    /// </summary>
    [Fact]
    public async Task GetProductCategoryNames_WhenCalled_ReturnsTheCategoryNames()
    {
        var categories = new[]
        {
            CategoryUtils.CreateCategory(name: "category1"),
            CategoryUtils.CreateCategory(name: "category2"),
        };

        var product = ProductUtils.CreateProduct(
            categories: categories.Select(c => ProductCategory.Create(c.Id))
        );

        var expectedCategoryNames = categories.Select(c => c.Name);

        _mockCategoryRepository
            .Setup(r => r.FindAllAsync(It.IsAny<Expression<Func<Category, bool>>>()))
            .ReturnsAsync(categories);

        var result = await _service.GetProductCategoryNamesAsync(product);

        result.Should().BeEquivalentTo(expectedCategoryNames);
    }

    /// <summary>
    /// Verifies the calculation of the product price applying sales.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task CalculatePriceApplyingSale_WhenCalled_ReturnsThePrice()
    {
        var product = ProductUtils.CreateProduct(
            id: ProductId.Create(1),
            basePrice: 100
        );

        var productSales = new[]
        {
            SaleUtils.CreateSale(
                discount: DiscountUtils.CreateDiscount(
                    percentage: PercentageUtils.Create(10),
                    startingDate: DateTimeOffset.UtcNow.AddHours(-5),
                    endingDate: DateTimeOffset.UtcNow.AddHours(5)
                ),
                productsInSale: new HashSet<ProductReference>()
                {
                    ProductReference.Create(product.Id)
                }
            )
        };

        var expectedProductPriceAfterDiscountsApplied = 90;

        _mockSaleService
            .Setup(s => s.GetProductSalesAsync(It.IsAny<SaleProduct>()))
            .ReturnsAsync(productSales);

        var result = await _service.CalculateProductPriceApplyingSaleAsync(product);

        result.Should().Be(expectedProductPriceAfterDiscountsApplied);
    }
}
