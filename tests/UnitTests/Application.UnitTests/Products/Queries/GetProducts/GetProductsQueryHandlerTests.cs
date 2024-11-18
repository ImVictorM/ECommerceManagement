using System.Linq.Expressions;
using Application.Common.Interfaces.Persistence;
using Application.Products.Queries.GetProducts;
using Application.UnitTests.Products.Queries.TestUtils;
using Domain.ProductAggregate;
using Domain.ProductAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Products.Queries.GetProducts;

/// <summary>
/// Unit tests for the <see cref="GetProductsQueryHandler"/> handler.
/// </summary>
public class GetProductsQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRepository<Product, ProductId>> _mockProductRepository;
    private readonly GetProductsQueryHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetProductsQueryHandlerTests"/> class,
    /// </summary>
    public GetProductsQueryHandlerTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockProductRepository = new Mock<IRepository<Product, ProductId>>();

        _mockUnitOfWork.Setup(uow => uow.ProductRepository).Returns(_mockProductRepository.Object);

        _handler = new GetProductsQueryHandler(_mockUnitOfWork.Object);
    }

    /// <summary>
    /// Tests that getting the products without specifying a limit retrieves the first 20 products.
    /// </summary>
    [Fact]
    public async Task HandleGetAllProducts_WhenGettingProductsWithoutSpecifyingLimit_RetrievesFirst20Products()
    {
        var defaultQuantityToRetrieve = 20;
        var thirtyProducts = ProductUtils.CreateProducts(count: 30).ToList();
        var expectedProducts = thirtyProducts.Take(defaultQuantityToRetrieve).ToList();
        var query = GetProductsQueryUtils.CreateQuery();

        _mockProductRepository
            .Setup(r => r.FindAllAsync(It.IsAny<Expression<Func<Product, bool>>>()))
            .ReturnsAsync(thirtyProducts);

        var result = await _handler.Handle(query, default);

        result.Products.Count().Should().Be(defaultQuantityToRetrieve);
        result.Products.Should().BeEquivalentTo(expectedProducts);
    }

    /// <summary>
    /// Tests that it is possible to fetch products including a custom limit.
    /// </summary>
    [Fact]
    public async Task HandleGetAllProducts_WhenGettingProductsSpecifyingLimit_RetrievesProductsSubset()
    {
        var quantityToFetch = 5;
        var tenProducts = ProductUtils.CreateProducts(count: 10).ToList();
        var expectedProducts = tenProducts.Take(quantityToFetch).ToList();
        var query = GetProductsQueryUtils.CreateQuery(limit: quantityToFetch);

        _mockProductRepository
            .Setup(r => r.FindAllAsync(It.IsAny<Expression<Func<Product, bool>>>()))
            .ReturnsAsync(tenProducts);

        var result = await _handler.Handle(query, default);

        result.Products.Count().Should().Be(quantityToFetch);
        result.Products.Should().BeEquivalentTo(expectedProducts);
    }
}
