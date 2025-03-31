using Application.Common.Persistence.Repositories;
using Application.ShippingMethods.Queries.GetShippingMethodById;
using Application.ShippingMethods.Errors;
using Application.UnitTests.ShippingMethods.Queries.TestUtils;

using Domain.ShippingMethodAggregate.ValueObjects;
using Domain.ShippingMethodAggregate;
using Domain.UnitTests.TestUtils;

using Microsoft.Extensions.Logging;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.ShippingMethods.Queries.GetShippingMethodById;

/// <summary>
/// Unit tests for the <see cref="GetShippingMethodByIdQueryHandler"/>
/// query handler.
/// </summary>
public class GetShippingMethodByIdQueryHandlerTests
{
    private readonly GetShippingMethodByIdQueryHandler _handler;
    private readonly Mock<IShippingMethodRepository> _mockShippingMethodRepository;

    /// <summary>
    /// Initiates a new instance of the
    /// <see cref="GetShippingMethodByIdQueryHandlerTests"/> class.
    /// </summary>
    public GetShippingMethodByIdQueryHandlerTests()
    {
        _mockShippingMethodRepository = new Mock<IShippingMethodRepository>();

        _handler = new GetShippingMethodByIdQueryHandler(
            _mockShippingMethodRepository.Object,
            Mock.Of<ILogger<GetShippingMethodByIdQueryHandler>>()
        );
    }

    /// <summary>
    /// Verifies the shipping method is returned when it exists.
    /// </summary>
    [Fact]
    public async Task HandleGetShippingMethodByIdQuery_WithExistentShippingMethod_ReturnsIt()
    {
        var request = GetShippingMethodByIdQueryUtils.CreateQuery();
        var shippingMethod = ShippingMethodUtils.CreateShippingMethod(
            id: ShippingMethodId.Create(request.ShippingMethodId)
        );

        _mockShippingMethodRepository
            .Setup(r => r.FindByIdAsync(
                It.IsAny<ShippingMethodId>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(shippingMethod);

        var result = await _handler.Handle(request, default);

        result.Id.Should().Be(shippingMethod.Id.ToString());
        result.Name.Should().Be(shippingMethod.Name);
        result.Price.Should().Be(shippingMethod.Price);
        result.EstimatedDeliveryDays
            .Should().Be(shippingMethod.EstimatedDeliveryDays);
    }

    /// <summary>
    /// Verifies an error is thrown when the shipping method does not exist.
    /// </summary>
    [Fact]
    public async Task HandleGetShippingMethodByIdQuery_WithNonExistentShippingMethod_ThrowsError()
    {
        var request = GetShippingMethodByIdQueryUtils.CreateQuery();

        _mockShippingMethodRepository
            .Setup(r => r.FindByIdAsync(
                It.IsAny<ShippingMethodId>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync((ShippingMethod?)null);

        await FluentActions
            .Invoking(() => _handler.Handle(request, default))
            .Should()
            .ThrowAsync<ShippingMethodNotFoundException>();
    }
}
