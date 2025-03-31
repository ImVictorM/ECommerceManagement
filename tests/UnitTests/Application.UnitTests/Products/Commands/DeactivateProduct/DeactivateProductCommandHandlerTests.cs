using Application.Common.Persistence.Repositories;
using Application.Common.Persistence;
using Application.Products.Commands.DeactivateProduct;
using Application.Products.Errors;
using Application.UnitTests.Products.Commands.TestUtils;

using Domain.ProductAggregate;
using Domain.ProductAggregate.Specifications;
using Domain.UnitTests.TestUtils;

using SharedKernel.Interfaces;

using Microsoft.Extensions.Logging;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Products.Commands.DeactivateProduct;

/// <summary>
/// Unit tests for the <see cref="DeactivateProductCommandHandler"/> class.
/// </summary>
public class DeactivateProductCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly DeactivateProductCommandHandler _handler;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="DeactivateProductCommandHandlerTests"/> class,
    /// </summary>
    public DeactivateProductCommandHandlerTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockProductRepository = new Mock<IProductRepository>();

        _handler = new DeactivateProductCommandHandler(
            _mockUnitOfWork.Object,
            _mockProductRepository.Object,
            Mock.Of<ILogger<DeactivateProductCommandHandler>>()
        );
    }

    /// <summary>
    /// Verifies an exception is thrown when the product to be deactivate does not
    /// exist or is already inactive.
    /// </summary>
    [Fact]
    public async Task HandleDeactivateProductCommand_WhenProductDoesNotExist_ThrowsError()
    {
        _mockProductRepository
            .Setup(r => r.FindFirstSatisfyingAsync(
                It.IsAny<ISpecificationQuery<Product>>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync((Product?)null);

        var command = DeactivateProductCommandUtils.CreateCommand();

        await FluentActions
            .Invoking(() => _handler.Handle(command, default))
            .Should()
            .ThrowAsync<ProductNotFoundException>();
    }

    /// <summary>
    /// Verifies when the product exists it is deactivated and the quantity
    /// in inventory is set to 0.
    /// </summary>
    [Fact]
    public async Task HandleDeactivateProductCommand_WhenProductExists_ShouldDeactivateAndClearStock()
    {
        var productToBeDeactivate = ProductUtils.CreateProduct();

        _mockProductRepository
            .Setup(r => r.FindFirstSatisfyingAsync(
                It.IsAny<QueryActiveProductByIdSpecification>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(productToBeDeactivate);

        var command = DeactivateProductCommandUtils.CreateCommand();

        await _handler.Handle(command, default);

        productToBeDeactivate.IsActive.Should().BeFalse();
        productToBeDeactivate.Inventory.QuantityAvailable.Should().Be(0);
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
    }
}
