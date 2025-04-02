using Application.Common.Persistence.Repositories;
using Application.Common.DTOs.Pagination;
using Application.Products.Queries.GetProducts;
using Application.UnitTests.Products.Queries.TestUtils;

using Domain.ProductAggregate;
using Domain.ProductAggregate.Services;
using Domain.ProductAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;

using SharedKernel.Interfaces;

using Microsoft.Extensions.Logging;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Products.Queries.GetProducts;

/// <summary>
/// Unit tests for the <see cref="GetProductsQueryHandler"/> handler.
/// </summary>
public class GetProductsQueryHandlerTests
{
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly Mock<IProductPricingService> _mockProductPricingService;
    private readonly GetProductsQueryHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetProductsQueryHandlerTests"/>
    /// class.
    /// </summary>
    public GetProductsQueryHandlerTests()
    {
        _mockProductRepository = new Mock<IProductRepository>();
        _mockProductPricingService = new Mock<IProductPricingService>();

        _handler = new GetProductsQueryHandler(
            _mockProductRepository.Object,
            _mockProductPricingService.Object,
            new Mock<ILogger<GetProductsQueryHandler>>().Object
        );
    }

    /// <summary>
    /// Verifies the products are retrieved correctly with default pagination.
    /// </summary>
    [Fact]
    public async Task HandleGetProductsQuery_WithValidRequest_ReturnsProductResults()
    {
        var query = GetProductsQueryUtils.CreateQuery();

        var products = new[]
        {
            ProductUtils.CreateProduct(id: ProductId.Create(1)),
            ProductUtils.CreateProduct(id: ProductId.Create(2))
        };

        var productPrices = products.ToDictionary(
            p => p.Id,
            p => p.BasePrice
        );

        _mockProductRepository
            .Setup(r => r.GetProductsSatisfyingAsync(
                It.IsAny<ISpecificationQuery<Product>>(),
                It.IsAny<PaginationParams>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(products);

        _mockProductPricingService
            .Setup(s => s.CalculateDiscountedPricesAsync(
                products,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(productPrices);

        var result = await _handler.Handle(query, default);

        _mockProductRepository.Verify(r => r.GetProductsSatisfyingAsync(
            It.IsAny<ISpecificationQuery<Product>>(),
            It.Is<PaginationParams>(p => p.Page == 1 && p.PageSize == 20),
            It.IsAny<CancellationToken>()
        ));

        result.Select(p => p.Id).Should()
            .BeEquivalentTo(products.Select(p => p.Id.ToString()));
    }
}
