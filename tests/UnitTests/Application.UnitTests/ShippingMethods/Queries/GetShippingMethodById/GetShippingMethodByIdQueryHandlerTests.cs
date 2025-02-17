using Application.Common.Persistence;
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
/// Unit tests for the <see cref="GetShippingMethodByIdQueryHandler"/> query handler.
/// </summary>
public class GetShippingMethodByIdQueryHandlerTests
{
    private readonly GetShippingMethodByIdQueryHandler _handler;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRepository<ShippingMethod, ShippingMethodId>> _mockShippingMethodRepository;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetShippingMethodByIdQueryHandlerTests"/> class.
    /// </summary>
    public GetShippingMethodByIdQueryHandlerTests()
    {
        _mockShippingMethodRepository = new Mock<IRepository<ShippingMethod, ShippingMethodId>>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _mockUnitOfWork.Setup(uow => uow.ShippingMethodRepository).Returns(_mockShippingMethodRepository.Object);

        _handler = new GetShippingMethodByIdQueryHandler(
            _mockUnitOfWork.Object,
            new Mock<ILogger<GetShippingMethodByIdQueryHandler>>().Object
        );
    }

    /// <summary>
    /// Verifies the shipping method is returned when it exists.
    /// </summary>
    [Fact]
    public async Task HandleGetShippingMethodById_WithExistingShippingMethod_ReturnsIt()
    {
        var request = GetShippingMethodByIdQueryUtils.CreateQuery();
        var shippingMethod = ShippingMethodUtils.CreateShippingMethod(id: ShippingMethodId.Create(request.ShippingMethodId));

        _mockShippingMethodRepository
            .Setup(r => r.FindByIdAsync(It.IsAny<ShippingMethodId>()))
            .ReturnsAsync(shippingMethod);

        var result = await _handler.Handle(request, default);

        result.ShippingMethod.Should().BeEquivalentTo(shippingMethod);
    }

    /// <summary>
    /// Verifies an error is thrown when the shipping method does not exist.
    /// </summary>
    [Fact]
    public async Task HandleGetShippingMethodById_WithNonexistingShippingMethod_ThrowsError()
    {
        var request = GetShippingMethodByIdQueryUtils.CreateQuery();

        _mockShippingMethodRepository
            .Setup(r => r.FindByIdAsync(It.IsAny<ShippingMethodId>()))
            .ReturnsAsync((ShippingMethod?)null);

        await FluentActions
            .Invoking(() => _handler.Handle(request, default))
            .Should()
            .ThrowAsync<ShippingMethodNotFoundException>();
    }
}
