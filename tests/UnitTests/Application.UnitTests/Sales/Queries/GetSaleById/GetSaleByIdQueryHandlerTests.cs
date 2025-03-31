using Application.Common.Persistence.Repositories;
using Application.Sales.Errors;
using Application.Sales.Queries.GetSaleById;
using Application.UnitTests.Sales.Queries.TestUtils;

using Domain.SaleAggregate;
using Domain.SaleAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;

using Microsoft.Extensions.Logging;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Sales.Queries.GetSaleById;

/// <summary>
/// Unit tests for the <see cref="GetSaleByIdQueryHandler"/> query handler.
/// </summary>
public class GetSaleByIdQueryHandlerTests
{
    private readonly Mock<ISaleRepository> _mockSaleRepository;
    private readonly GetSaleByIdQueryHandler _handler;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="GetSaleByIdQueryHandlerTests"/> class.
    /// </summary>
    public GetSaleByIdQueryHandlerTests()
    {
        _mockSaleRepository = new Mock<ISaleRepository>();
        _handler = new GetSaleByIdQueryHandler(
            _mockSaleRepository.Object,
            Mock.Of<ILogger<GetSaleByIdQueryHandler>>()
        );
    }

    /// <summary>
    /// Verifies a sale is successfully retrieved when it exists.
    /// </summary>
    [Fact]
    public async Task HandleGetSaleByIdQuery_WithExistentSale_ReturnsSaleResult()
    {
        var saleId = "1";
        var sale = SaleUtils.CreateSale(id: SaleId.Create(saleId));
        var request = GetSaleByIdQueryUtils.CreateQuery(saleId: saleId);

        _mockSaleRepository
            .Setup(r => r.FindByIdAsync(
                sale.Id,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(sale);

        var result = await _handler.Handle(request, default);

        result.Should().NotBeNull();
        result.Id.Should().Be(sale.Id.ToString());
        result.Discount.Should().Be(sale.Discount);
        result.CategoryOnSaleIds.Should().BeEquivalentTo(
            sale.CategoriesOnSale.Select(c => c.CategoryId.ToString())
        );
        result.ProductOnSaleIds.Should().BeEquivalentTo(
            sale.ProductsOnSale.Select(p => p.ProductId.ToString())
        );
        result.ProductExcludedFromSaleIds.Should().BeEquivalentTo(
            sale.ProductsExcludedFromSale.Select(p => p.ProductId.ToString())
        );
    }

    /// <summary>
    /// Verifies an exception is thrown when trying to retrieve a non-existent sale.
    /// </summary>
    [Fact]
    public async Task HandleGetSaleByIdQuery_WithNonExistentSale_ThrowsError()
    {
        var saleId = SaleId.Create(1);
        var request = GetSaleByIdQueryUtils.CreateQuery(saleId.ToString());

        _mockSaleRepository
            .Setup(r => r.FindByIdAsync(
                saleId,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync((Sale?)null);

        await FluentActions
            .Invoking(() => _handler.Handle(request, default))
            .Should()
            .ThrowAsync<SaleNotFoundException>();
    }
}
