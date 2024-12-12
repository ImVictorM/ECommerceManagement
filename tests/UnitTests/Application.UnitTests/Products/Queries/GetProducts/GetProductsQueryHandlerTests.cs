using Application.Common.Interfaces.Persistence;
using Application.Products.Queries.GetProducts;
using Application.UnitTests.Products.Queries.TestUtils;
using Domain.ProductAggregate;
using Domain.ProductAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;
using Microsoft.Extensions.Logging;
using Moq;
using SharedKernel.Models;

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

        _handler = new GetProductsQueryHandler(
            _mockUnitOfWork.Object,
            new Mock<ILogger<GetProductsQueryHandler>>().Object
        );
    }

    /// <summary>
    /// Tests that getting the products without specifying a limit retrieves the first 20 products.
    /// </summary>
    [Fact]
    public async Task HandleGetAllProducts_WhenGettingProductsWithoutSpecifyingLimit_RetrievesFirst20Products()
    {

        _mockProductRepository
            .Setup(r => r.FindSatisfyingAsync(It.IsAny<CompositeQuerySpecification<Product>>(), It.IsAny<int>()))
            .ReturnsAsync(ProductUtils.CreateProducts());

        await _handler.Handle(GetProductsQueryUtils.CreateQuery(), default);

        _mockProductRepository.Verify(r => r.FindSatisfyingAsync(It.IsAny<CompositeQuerySpecification<Product>>(), 20));
    }

    /// <summary>
    /// Tests that it is possible to fetch products including a custom limit.
    /// </summary>
    [Fact]
    public async Task HandleGetAllProducts_WhenGettingProductsSpecifyingLimit_PassesParameterCorrectly()
    {
        var quantityToFetch = 5;
        var query = GetProductsQueryUtils.CreateQuery(limit: quantityToFetch);

        _mockProductRepository
            .Setup(r => r.FindSatisfyingAsync(It.IsAny<CompositeQuerySpecification<Product>>(), It.IsAny<int>()))
            .ReturnsAsync(ProductUtils.CreateProducts());

        await _handler.Handle(query, default);

        _mockProductRepository.Verify(r => r.FindSatisfyingAsync(It.IsAny<CompositeQuerySpecification<Product>>(), quantityToFetch));
    }
}
