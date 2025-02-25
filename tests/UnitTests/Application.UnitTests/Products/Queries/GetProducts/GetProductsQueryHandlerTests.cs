using Application.Common.Persistence.Repositories;
using Application.Products.DTOs;
using Application.Products.Queries.GetProducts;
using Application.UnitTests.Products.Queries.TestUtils;

using Domain.ProductAggregate;
using Domain.ProductAggregate.Services;
using Domain.ProductAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;

using SharedKernel.Models;

using Microsoft.Extensions.Logging;
using Moq;

namespace Application.UnitTests.Products.Queries.GetProducts;

/// <summary>
/// Unit tests for the <see cref="GetProductsQueryHandler"/> handler.
/// </summary>
public class GetProductsQueryHandlerTests
{
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly Mock<IProductService> _mockProductService;
    private readonly GetProductsQueryHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetProductsQueryHandlerTests"/> class,
    /// </summary>
    public GetProductsQueryHandlerTests()
    {
        _mockProductRepository = new Mock<IProductRepository>();
        _mockProductService = new Mock<IProductService>();

        _handler = new GetProductsQueryHandler(
            _mockProductRepository.Object,
            _mockProductService.Object,
            new Mock<ILogger<GetProductsQueryHandler>>().Object
        );
    }

    /// <summary>
    /// Tests that getting the products without specifying pagination details retrieves the first 20 products.
    /// </summary>
    [Fact]
    public async Task HandleGetAllProducts_WithoutSpecifyingPagination_RetrievesFirstTwentyProducts()
    {
        var query = GetProductsQueryUtils.CreateQuery();

        var queryResult = new[]
        {
            new ProductWithCategoriesQueryResult(
                ProductUtils.CreateProduct(id: ProductId.Create(1)),
                ["category_1", "category_2"]
            ),

            new ProductWithCategoriesQueryResult(
                ProductUtils.CreateProduct(id: ProductId.Create(2)),
                ["category_1"]
            ),
        };

        var products = queryResult.Select(qr => qr.Product);

        var productPrices = products.ToDictionary(
            p => p.Id,
            p => p.BasePrice
        );

        _mockProductRepository
            .Setup(r => r.GetProductsWithCategoriesSatisfyingAsync(
                It.IsAny<CompositeQuerySpecification<Product>>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(queryResult);

        _mockProductService
            .Setup(s => s.CalculateProductsPriceApplyingSaleAsync(
                products,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(productPrices);

        await _handler.Handle(query, default);

        _mockProductRepository.Verify(r => r.GetProductsWithCategoriesSatisfyingAsync(
            It.IsAny<CompositeQuerySpecification<Product>>(),
            1,
            20,
            It.IsAny<CancellationToken>()
        ));
    }

    /// <summary>
    /// Tests that it is possible to fetch products including pagination details.
    /// </summary>
    [Fact]
    public async Task HandleGetAllProducts_SpecifyingPaginationDetails_RetrievesWithPagination()
    {
        var page = 1;
        var pageSize = 5;

        var query = GetProductsQueryUtils.CreateQuery(
            page: page,
            pageSize: pageSize
        );

        var queryResult = new[]
        {
            new ProductWithCategoriesQueryResult(
                ProductUtils.CreateProduct(id: ProductId.Create(1)),
                ["category_1", "category_2"]
            ),

            new ProductWithCategoriesQueryResult(
                ProductUtils.CreateProduct(id: ProductId.Create(2)),
                ["category_1"]
            ),
        };

        var products = queryResult.Select(qr => qr.Product);

        var productPrices = products.ToDictionary(
            p => p.Id,
            p => p.BasePrice
        );

        _mockProductRepository
            .Setup(r => r.GetProductsWithCategoriesSatisfyingAsync(
                It.IsAny<CompositeQuerySpecification<Product>>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(queryResult);

        _mockProductService
            .Setup(s => s.CalculateProductsPriceApplyingSaleAsync(
                products,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(productPrices);

        await _handler.Handle(query, default);

        _mockProductRepository.Verify(r => r.GetProductsWithCategoriesSatisfyingAsync(
            It.IsAny<CompositeQuerySpecification<Product>>(),
            page,
            pageSize,
            It.IsAny<CancellationToken>()
        ));
    }
}
