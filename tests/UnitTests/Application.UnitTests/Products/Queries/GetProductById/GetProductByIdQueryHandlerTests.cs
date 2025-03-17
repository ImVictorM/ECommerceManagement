using Application.Common.Persistence.Repositories;
using Application.Products.DTOs;
using Application.Products.Errors;
using Application.Products.Queries.GetProductById;
using Application.UnitTests.Products.Queries.TestUtils;

using Domain.CategoryAggregate.ValueObjects;
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
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly Mock<IProductPricingService> _mockProductPricingService;
    private readonly GetProductByIdQueryHandler _handler;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="GetProductByIdQueryHandlerTests"/> class.
    /// </summary>
    public GetProductByIdQueryHandlerTests()
    {
        _mockProductRepository = new Mock<IProductRepository>();
        _mockProductPricingService = new Mock<IProductPricingService>();

        _handler = new GetProductByIdQueryHandler(
            _mockProductRepository.Object,
            _mockProductPricingService.Object,
            new Mock<ILogger<GetProductByIdQueryHandler>>().Object
        );
    }

    /// <summary>
    /// Verifies the product with details is returned when the product exists.
    /// </summary>
    [Fact]
    public async Task HandleGetProductById_WithExistingProduct_ReturnsProductResult()
    {
        var productToFind = ProductUtils.CreateProduct(
            id: ProductId.Create(1),
            basePrice: 20m,
            categories:
            [
                ProductCategory.Create(CategoryId.Create(1)),
                ProductCategory.Create(CategoryId.Create(2))
            ]
        );

        var productPriceWithDiscount = 15m;
        IEnumerable<string> productCategoryNames = ["tech", "home"];

        var query = GetProductByIdQueryUtils.CreateQuery(
            id: productToFind.Id.ToString()

        );

        _mockProductRepository
            .Setup(r => r.GetProductWithCategoriesSatisfyingAsync(
                It.IsAny<QueryActiveProductByIdSpecification>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(new ProductWithCategoriesQueryResult(
                productToFind,
                productCategoryNames
            ));

        _mockProductPricingService
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
    /// Verifies an exception is thrown when the product being queried does not
    /// exist.
    /// </summary>
    [Fact]
    public async Task HandleGetProductById_WithNonexistentProduct_ThrowsError()
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
