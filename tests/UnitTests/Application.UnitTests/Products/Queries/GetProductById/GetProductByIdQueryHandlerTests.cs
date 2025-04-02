using Application.Common.Persistence.Repositories;
using Application.Products.Errors;
using Application.Products.Queries.GetProductById;
using Application.UnitTests.Products.Queries.TestUtils;

using Domain.CategoryAggregate.ValueObjects;
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
            Mock.Of<ILogger<GetProductByIdQueryHandler>>()
        );
    }

    /// <summary>
    /// Verifies the product with details is returned when the product exists.
    /// </summary>
    [Fact]
    public async Task HandleGetProductByIdQuery_WithExistentProduct_ReturnsProductResult()
    {
        var product = ProductUtils.CreateProduct(
            id: ProductId.Create(1),
            basePrice: 20m,
            categories:
            [
                ProductCategory.Create(CategoryId.Create(1)),
                ProductCategory.Create(CategoryId.Create(2))
            ]
        );

        var productPriceWithDiscount = 15m;

        var query = GetProductByIdQueryUtils.CreateQuery(
            id: product.Id.ToString()

        );

        _mockProductRepository
            .Setup(r => r.FindFirstSatisfyingAsync(
                It.IsAny<QueryActiveProductByIdSpecification>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(product);

        _mockProductPricingService
            .Setup(s => s.CalculateDiscountedPriceAsync(
                product,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(productPriceWithDiscount);

        var result = await _handler.Handle(query, default);

        result.Id.Should().Be(product.Id.ToString());
        result.Name.Should().Be(product.Name);
        result.Description.Should().Be(product.Description);
        result.BasePrice.Should().Be(product.BasePrice);
        result.PriceWithDiscount.Should().Be(productPriceWithDiscount);
        result.QuantityAvailable.Should().Be(product.Inventory.QuantityAvailable);
        result.CategoryIds.Should().BeEquivalentTo(
            product.ProductCategories
                .Select(c => c.CategoryId.ToString())
                .ToList()
        );
        result.Images.Should().BeEquivalentTo(
            product.ProductImages.Select(i => i.Uri).ToList()
        );
    }

    /// <summary>
    /// Verifies an exception is thrown when the product being queried does not
    /// exist.
    /// </summary>
    [Fact]
    public async Task HandleGetProductByIdQuery_WithNonExistentProduct_ThrowsError()
    {
        var notFoundId = "5";
        var query = GetProductByIdQueryUtils.CreateQuery(id: notFoundId);

        _mockProductRepository
            .Setup(r => r.FindFirstSatisfyingAsync(
                It.IsAny<QueryActiveProductByIdSpecification>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync((Product?)null!);

        await FluentActions
            .Invoking(() => _handler.Handle(query, default))
            .Should()
            .ThrowAsync<ProductNotFoundException>();
    }
}
