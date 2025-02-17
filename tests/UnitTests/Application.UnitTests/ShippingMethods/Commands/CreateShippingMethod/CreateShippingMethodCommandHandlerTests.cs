using Application.Common.Persistence;
using Application.ShippingMethods.Commands.CreateShippingMethod;
using Application.UnitTests.ShippingMethods.Commands.TestUtils;
using Application.UnitTests.TestUtils.Behaviors;

using Domain.ShippingMethodAggregate;
using Domain.ShippingMethodAggregate.ValueObjects;

using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.UnitTests.ShippingMethods.Commands.CreateShippingMethod;

/// <summary>
/// Unit tests for the <see cref="CreateShippingMethodCommandHandler"/> command handler.
/// </summary>
public class CreateShippingMethodCommandHandlerTests
{
    private readonly CreateShippingMethodCommandHandler _handler;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRepository<ShippingMethod, ShippingMethodId>> _mockShippingMethodRepository;

    /// <summary>
    /// Initiates a new instance of the <see cref="CreateShippingMethodCommandHandlerTests"/> class.
    /// </summary>
    public CreateShippingMethodCommandHandlerTests()
    {
        _mockShippingMethodRepository = new Mock<IRepository<ShippingMethod, ShippingMethodId>>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _mockUnitOfWork.Setup(uow => uow.ShippingMethodRepository).Returns(_mockShippingMethodRepository.Object);

        _handler = new CreateShippingMethodCommandHandler(
            _mockUnitOfWork.Object,
            new Mock<ILogger<CreateShippingMethodCommandHandler>>().Object
        );
    }

    /// <summary>
    /// Verifies a shipping method is created correctly and the created identifier is returned.
    /// </summary>
    [Fact]
    public async Task HandleCreateShippingMethod_WithValidCommand_ReturnsCreatedResult()
    {
        var shippingMethodCreatedId = ShippingMethodId.Create(1);

        var request = CreateShippingMethodCommandUtils.CreateCommand();

        MockEFCoreBehaviors.MockSetEntityIdBehavior(_mockShippingMethodRepository, _mockUnitOfWork, shippingMethodCreatedId);

        var result = await _handler.Handle(request, default);

        result.Id.Should().Be(shippingMethodCreatedId.ToString());

        _mockShippingMethodRepository.Verify(
            r => r.AddAsync(It.Is<ShippingMethod>(s =>
                s.Name == request.Name
                && s.EstimatedDeliveryDays == request.EstimatedDeliveryDays
                && s.Price == request.Price
            )),
            Times.Once()
        );

        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once());
    }
}
