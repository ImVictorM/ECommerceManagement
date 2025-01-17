using Application.Common.Errors;
using Application.Common.Persistence;
using Application.Products.Queries.GetProductById;
using Application.UnitTests.Products.Queries.TestUtils;

using Domain.ProductAggregate;
using Domain.ProductAggregate.Services;
using Domain.ProductAggregate.Specifications;
using Domain.ProductAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;

using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.UnitTests.Products.Queries.GetProductById;

/// <summary>
/// Unit tests for the <see cref="GetProductByIdQueryHandler"/> handler.
/// </summary>
public class GetProductByIdQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRepository<Product, ProductId>> _mockProductRepository;
    private readonly Mock<IProductService> _mockProductService;
    private readonly GetProductByIdQueryHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetProductByIdQueryHandlerTests"/> class,
    /// </summary>
    public GetProductByIdQueryHandlerTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockProductRepository = new Mock<IRepository<Product, ProductId>>();
        _mockProductService = new Mock<IProductService>();

        _mockUnitOfWork.Setup(uow => uow.ProductRepository).Returns(_mockProductRepository.Object);

        _handler = new GetProductByIdQueryHandler(
            _mockUnitOfWork.Object,
            _mockProductService.Object,
            new Mock<ILogger<GetProductByIdQueryHandler>>().Object
        );
    }

    /// <summary>
    /// Tests that when the product being retrieved exists it is returned calculating its price and retrieving its category names.
    /// </summary>
    [Fact]
    public async Task HandleGetProductById_WhenProductExists_ReturnsProductResult()
    {
        var query = GetProductByIdQueryUtils.CreateQuery(id: "1");

        var productToFind = ProductUtils.CreateProduct(basePrice: 20m);
        var productPriceWithDiscount = 15m;
        IEnumerable<string> productCategoryNames = ["tech", "home"];

        _mockProductRepository
            .Setup(r => r.FindFirstSatisfyingAsync(It.IsAny<QueryActiveProductByIdSpecification>()))
            .ReturnsAsync(productToFind);

        _mockProductService
            .Setup(s => s.CalculateProductPriceApplyingSaleAsync(productToFind))
            .ReturnsAsync(productPriceWithDiscount);

        _mockProductService
            .Setup(s => s.GetProductCategoryNamesAsync(productToFind))
            .ReturnsAsync(productCategoryNames);

        var result = await _handler.Handle(query, default);

        result.Product.Should().BeEquivalentTo(productToFind);
        result.Categories.Should().BeEquivalentTo(productCategoryNames);
        result.PriceWithDiscount.Should().Be(productPriceWithDiscount);
    }

    /// <summary>
    /// Tests that when the product being retrieved does not exist throws a not found error.
    /// </summary>
    [Fact]
    public async Task HandleGetProductById_WhenProductDoesNotExistOrIsInactive_ThrowsNotFoundError()
    {
        var notFoundId = "5";
        var query = GetProductByIdQueryUtils.CreateQuery(id: notFoundId);

        _mockProductRepository
            .Setup(r => r.FindFirstSatisfyingAsync(It.IsAny<QueryActiveProductByIdSpecification>()))
            .ReturnsAsync((Product?)null!);

        await FluentActions
            .Invoking(() => _handler.Handle(query, default))
            .Should()
            .ThrowAsync<ProductNotFoundException>()
            .WithMessage($"The product with id {notFoundId} does not exist");
    }
}
