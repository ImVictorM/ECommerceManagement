using Application.Common.Persistence;
using Application.ShippingMethods.Queries.GetShippingMethods;
using Application.UnitTests.ShippingMethods.Queries.TestUtils;

using Domain.ShippingMethodAggregate;
using Domain.UnitTests.TestUtils;

using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.ShippingMethods.Queries.GetShippingMethods;

/// <summary>
/// Unit tests for the <see cref="GetShippingMethodsQueryHandler"/> query handler.
/// </summary>
public class GetShippingMethodsQueryHandlerTests
{
    private readonly GetShippingMethodsQueryHandler _handler;
    private readonly Mock<IShippingMethodRepository> _mockShippingMethodRepository;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetShippingMethodsQueryHandlerTests"/> class.
    /// </summary>
    public GetShippingMethodsQueryHandlerTests()
    {
        _mockShippingMethodRepository = new Mock<IShippingMethodRepository>();

        _handler = new GetShippingMethodsQueryHandler(
            _mockShippingMethodRepository.Object,
            new Mock<ILogger<GetShippingMethodsQueryHandler>>().Object
        );
    }

    /// <summary>
    /// Verifies all the shipping methods are returned.
    /// </summary>
    [Fact]
    public async Task HandleGetShippingMethods_WithValidCommand_ReturnsAllTheAvailableShippingMethods()
    {
        var request = GetShippingMethodsQueryUtils.CreateQuery();

        var shippingMethods = new[]
        {
            ShippingMethodUtils.CreateShippingMethod(),
            ShippingMethodUtils.CreateShippingMethod(),
            ShippingMethodUtils.CreateShippingMethod(),
        };

        _mockShippingMethodRepository
            .Setup(r => r.FindAllAsync(
                It.IsAny<Expression<Func<ShippingMethod, bool>>>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(shippingMethods);

        var result = await _handler.Handle(request, default);

        result.Select(r => r.ShippingMethod).Should().BeEquivalentTo(shippingMethods);

        _mockShippingMethodRepository.Verify(r =>
            r.FindAllAsync(
                It.IsAny<Expression<Func<ShippingMethod, bool>>>(),
                It.IsAny<CancellationToken>()
            ),
            Times.Once()
        );
    }
}
