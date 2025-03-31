using Application.Common.Persistence;
using Application.Common.Persistence.Repositories;
using Application.ShippingMethods.Commands.CreateShippingMethod;
using Application.UnitTests.ShippingMethods.Commands.TestUtils;
using Application.UnitTests.TestUtils.Extensions;

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
    private readonly Mock<IShippingMethodRepository> _mockShippingMethodRepository;

    /// <summary>
    /// Initiates a new instance of the
    /// <see cref="CreateShippingMethodCommandHandlerTests"/> class.
    /// </summary>
    public CreateShippingMethodCommandHandlerTests()
    {
        _mockShippingMethodRepository = new Mock<IShippingMethodRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _handler = new CreateShippingMethodCommandHandler(
            _mockUnitOfWork.Object,
            _mockShippingMethodRepository.Object,
            Mock.Of<ILogger<CreateShippingMethodCommandHandler>>()
        );
    }

    /// <summary>
    /// Verifies a shipping method is created correctly and the created
    /// identifier is returned.
    /// </summary>
    [Fact]
    public async Task HandleCreateShippingMethod_WithValidCommand_ReturnsCreatedResult()
    {
        var shippingMethodCreatedId = ShippingMethodId.Create(1);

        var request = CreateShippingMethodCommandUtils.CreateCommand();

        _mockUnitOfWork.MockSetEntityIdBehavior
            <IShippingMethodRepository, ShippingMethod, ShippingMethodId>(
                _mockShippingMethodRepository,
                shippingMethodCreatedId
            );

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

        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
    }
}
