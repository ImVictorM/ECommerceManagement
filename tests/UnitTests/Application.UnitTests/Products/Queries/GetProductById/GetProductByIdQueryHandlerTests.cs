using Application.Common.Persistence;
using Application.Products.DTOs;
using Application.Products.Errors;
using Application.Products.Queries.GetProductById;
using Application.UnitTests.Products.Queries.TestUtils;

using Domain.ProductAggregate.Services;
using Domain.ProductAggregate.Specifications;
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
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly Mock<IProductService> _mockProductService;
    private readonly GetProductByIdQueryHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetProductByIdQueryHandlerTests"/> class,
    /// </summary>
    public GetProductByIdQueryHandlerTests()
    {
        _mockProductRepository = new Mock<IProductRepository>();
        _mockProductService = new Mock<IProductService>();

        _handler = new GetProductByIdQueryHandler(
            _mockProductRepository.Object,
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
            .Setup(r => r.GetProductWithCategoriesSatisfyingAsync(
                It.IsAny<QueryActiveProductByIdSpecification>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(new ProductWithCategoriesQueryResult(
                productToFind,
                productCategoryNames
            ));

        _mockProductService
            .Setup(s => s.CalculateProductPriceApplyingSaleAsync(
                productToFind,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(productPriceWithDiscount);

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
            .Setup(r => r.GetProductWithCategoriesSatisfyingAsync(
                It.IsAny<QueryActiveProductByIdSpecification>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync((ProductWithCategoriesQueryResult?)null!);

        await FluentActions
            .Invoking(() => _handler.Handle(query, default))
            .Should()
            .ThrowAsync<ProductNotFoundException>()
            .WithMessage($"The product with id {notFoundId} does not exist");
    }
}
