using Application.Common.Persistence;
using Application.ShippingMethods.Commands.DeleteShippingMethod;
using Application.ShippingMethods.Errors;
using Application.UnitTests.ShippingMethods.Commands.TestUtils;

using Domain.UnitTests.TestUtils;
using Domain.ShippingMethodAggregate.ValueObjects;
using Domain.ShippingMethodAggregate;

using Microsoft.Extensions.Logging;
using Moq;
using FluentAssertions;

namespace Application.UnitTests.ShippingMethods.Commands.DeleteShippingMethod;

/// <summary>
/// Unit tests for the <see cref="DeleteShippingMethodCommandHandler"/> command handler.
/// </summary>
public class DeleteShippingMethodCommandHandlerTests
{
    private readonly DeleteShippingMethodCommandHandler _handler;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRepository<ShippingMethod, ShippingMethodId>> _mockShippingMethodRepository;

    /// <summary>
    /// Initiates a new instance of the <see cref="DeleteShippingMethodCommandHandlerTests"/> class.
    /// </summary>
    public DeleteShippingMethodCommandHandlerTests()
    {
        _mockShippingMethodRepository = new Mock<IRepository<ShippingMethod, ShippingMethodId>>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _mockUnitOfWork.Setup(uow => uow.ShippingMethodRepository).Returns(_mockShippingMethodRepository.Object);

        _handler = new DeleteShippingMethodCommandHandler(
            _mockUnitOfWork.Object,
            new Mock<ILogger<DeleteShippingMethodCommandHandler>>().Object
        );
    }

    /// <summary>
    /// Verifies the shipping method is deleted when it exists.
    /// </summary>
    [Fact]
    public async Task HandleDeleteShippingMethod_WithExistingShippingMethod_DeletesIt()
    {
        var request = DeleteShippingMethodCommandUtils.CreateCommand();
        var shippingMethodToBeDeleted = ShippingMethodUtils.CreateShippingMethod(id: ShippingMethodId.Create(request.ShippingMethodId));

        _mockShippingMethodRepository
            .Setup(r => r.FindByIdAsync(It.IsAny<ShippingMethodId>()))
            .ReturnsAsync(shippingMethodToBeDeleted);

        await _handler.Handle(request, default);

        _mockShippingMethodRepository.Verify(r => r.RemoveOrDeactivate(shippingMethodToBeDeleted), Times.Once());
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once());
    }

    /// <summary>
    /// Verifies an error is thrown when the shipping method to be deleted does not exist.
    /// </summary>
    [Fact]
    public async Task HandleDeleteShippingMethod_WithNonexistingShippingMethod_ThrowsError()
    {
        var request = DeleteShippingMethodCommandUtils.CreateCommand();

        _mockShippingMethodRepository
            .Setup(r => r.FindByIdAsync(It.IsAny<ShippingMethodId>()))
            .ReturnsAsync((ShippingMethod?)null);

        await FluentActions
            .Invoking(() => _handler.Handle(request, default))
            .Should()
            .ThrowAsync<ShippingMethodNotFoundException>();
    }
}
