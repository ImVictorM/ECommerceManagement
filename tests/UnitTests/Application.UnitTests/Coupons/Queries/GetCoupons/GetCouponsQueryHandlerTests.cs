using Application.Common.Persistence.Repositories;
using Application.Coupons.Queries.GetCoupons;
using Application.Coupons.DTOs.Results;
using Application.UnitTests.Coupons.Queries.TestUtils;

using Domain.UnitTests.TestUtils;

using Microsoft.Extensions.Logging;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Coupons.Queries.GetCoupons;

/// <summary>
/// Unit tests for the <see cref="GetCouponsQueryHandler"/> handler.
/// </summary>
public class GetCouponsQueryHandlerTests
{
    private readonly GetCouponsQueryHandler _handler;
    private readonly Mock<ICouponRepository> _mockCouponRepository;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="GetCouponsQueryHandlerTests"/> class.
    /// </summary>
    public GetCouponsQueryHandlerTests()
    {
        _mockCouponRepository = new Mock<ICouponRepository>();

        _handler = new GetCouponsQueryHandler(
            _mockCouponRepository.Object,
            Mock.Of<ILogger<GetCouponsQueryHandler>>()
        );
    }

    /// <summary>
    /// Verifies when there are coupons matching the filters, they are returned.
    /// </summary>
    [Fact]
    public async Task HandleGetCouponsQuery_WithMatchingCoupons_ReturnsCoupons()
    {
        var query = GetCouponsQueryUtils.CreateQuery();

        var coupons = CouponUtils.CreateCoupons(count: 4);

        _mockCouponRepository
            .Setup(r => r.GetCouponsAsync(
                query.Filters,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(coupons);

        var result = await _handler.Handle(query, default);

        result.Should().HaveCount(coupons.Count);
        result.Should().AllBeOfType<CouponResult>();
    }

    /// <summary>
    /// Verifies when no coupons match the filters, an empty collection is returned.
    /// </summary>
    [Fact]
    public async Task HandleGetCouponsQuery_WithNoMatchingCoupons_ReturnsEmptyCollection()
    {
        var query = GetCouponsQueryUtils.CreateQuery();

        _mockCouponRepository
            .Setup(r => r.GetCouponsAsync(
                query.Filters,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync([]);

        var result = await _handler.Handle(query, default);

        result.Should().BeEmpty();
    }
}
