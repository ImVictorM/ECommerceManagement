using Application.Common.Persistence.Repositories;
using Application.Sales.DTOs.Filters;
using Application.Sales.DTOs.Results;
using Application.Sales.Queries.GetSales;
using Application.UnitTests.Sales.Queries.TestUtils;

using Domain.UnitTests.TestUtils;

using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.UnitTests.Sales.Queries.GetSales;

/// <summary>
/// Unit tests for the <see cref="GetSalesQueryHandler"/> query handler.
/// </summary>
public class GetSalesQueryHandlerTests
{
    private readonly Mock<ISaleRepository> _mockSaleRepository;
    private readonly GetSalesQueryHandler _handler;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="GetSalesQueryHandlerTests"/> class.
    /// </summary>
    public GetSalesQueryHandlerTests()
    {
        _mockSaleRepository = new Mock<ISaleRepository>();
        _handler = new GetSalesQueryHandler(
            _mockSaleRepository.Object,
            Mock.Of<ILogger<GetSalesQueryHandler>>()
        );
    }

    /// <summary>
    /// Verifies sales are successfully retrieved when they exist.
    /// </summary>
    [Fact]
    public async Task HandleGetSalesQuery_WithExistentSales_ReturnsSaleResults()
    {
        var sales = SaleUtils.CreateSales(3);
        var request = GetSalesQueryUtils.CreateQuery();

        _mockSaleRepository
            .Setup(r => r.GetSalesAsync(
                It.IsAny<SaleFilters>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(sales);

        var result = await _handler.Handle(request, default);

        result.Should().HaveCount(sales.Count);
        result.Should().AllBeOfType<SaleResult>();
        result.Select(s => s.Id)
            .Should()
            .BeEquivalentTo(sales.Select(s => s.Id.ToString()));
    }
}
