using Application.Common.Persistence;
using Application.ShippingMethods.Commands.UpdateShippingMethod;
using Application.ShippingMethods.Errors;
using Application.UnitTests.ShippingMethods.Commands.TestUtils;

using Domain.ShippingMethodAggregate.ValueObjects;
using Domain.ShippingMethodAggregate;
using Domain.UnitTests.TestUtils;

using Microsoft.Extensions.Logging;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.ShippingMethods.Commands.UpdateShippingMethod;

/// <summary>
/// Unit tests for the <see cref="UpdateShippingMethodCommandHandler"/> command handler.
/// </summary>
public class UpdateShippingMethodCommandHandlerTests
{
    private readonly UpdateShippingMethodCommandHandler _handler;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IShippingMethodRepository> _mockShippingMethodRepository;

    /// <summary>
    /// Initiates a new instance of the <see cref="UpdateShippingMethodCommandHandlerTests"/> class.
    /// </summary>
    public UpdateShippingMethodCommandHandlerTests()
    {
        _mockShippingMethodRepository = new Mock<IShippingMethodRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _handler = new UpdateShippingMethodCommandHandler(
            _mockUnitOfWork.Object,
            _mockShippingMethodRepository.Object,
            new Mock<ILogger<UpdateShippingMethodCommandHandler>>().Object
        );
    }

    /// <summary>
    /// Verifies the shipping method is updated when it exists.
    /// </summary>
    [Fact]
    public async Task HandleUpdateShippingMethod_WithExistingShippingMethod_UpdatesIt()
    {
        var newName = "SuperFastUltraDeliveryMethod";
        var newPrice = 20m;
        var newEstimatedDeliveryDays = 3;

        var request = UpdateShippingMethodCommandUtils.CreateCommand(
            name: newName,
            price: newPrice,
            estimatedDeliveryDays: newEstimatedDeliveryDays
        );

        var shippingMethodToBeUpdated = ShippingMethodUtils.CreateShippingMethod(id: ShippingMethodId.Create(request.ShippingMethodId));

        _mockShippingMethodRepository
            .Setup(r => r.FindByIdAsync(
                It.IsAny<ShippingMethodId>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(shippingMethodToBeUpdated);

        await _handler.Handle(request, default);

        shippingMethodToBeUpdated.Name.Should().Be(newName);
        shippingMethodToBeUpdated.Price.Should().Be(newPrice);
        shippingMethodToBeUpdated.EstimatedDeliveryDays.Should().Be(newEstimatedDeliveryDays);

        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once());
    }

    /// <summary>
    /// Verifies an error is thrown when the shipping method to be updated does not exist.
    /// </summary>
    [Fact]
    public async Task HandleUpdateShippingMethod_WithNonexistingShippingMethod_ThrowsError()
    {
        var request = UpdateShippingMethodCommandUtils.CreateCommand();

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
