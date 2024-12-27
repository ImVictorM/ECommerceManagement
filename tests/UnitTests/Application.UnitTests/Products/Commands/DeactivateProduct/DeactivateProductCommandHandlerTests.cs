using Application.Common.Errors;
using Application.Common.Interfaces.Persistence;
using Application.Products.Commands.DeactivateProduct;
using Application.UnitTests.Products.Commands.TestUtils;

using Domain.ProductAggregate;
using Domain.ProductAggregate.Specifications;
using Domain.ProductAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;

using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using SharedKernel.Interfaces;

namespace Application.UnitTests.Products.Commands.DeactivateProduct;

/// <summary>
/// Unit tests for the <see cref="DeactivateProductCommandHandler"/> class.
/// </summary>
public class DeactivateProductCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRepository<Product, ProductId>> _mockProductRepository;
    private readonly DeactivateProductCommandHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeactivateProductCommandHandlerTests"/> class,
    /// </summary>
    public DeactivateProductCommandHandlerTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockProductRepository = new Mock<IRepository<Product, ProductId>>();

        _mockUnitOfWork.Setup(uow => uow.ProductRepository).Returns(_mockProductRepository.Object);

        _handler = new DeactivateProductCommandHandler(
            _mockUnitOfWork.Object,
            new Mock<ILogger<DeactivateProductCommandHandler>>().Object
        );
    }

    /// <summary>
    /// Tests when the product to be deactivate does not exist or is already inactive an error is thrown.
    /// </summary>
    [Fact]
    public async Task HandleDeactivateProduct_WhenProductDoesNotExist_ThrowsError()
    {
        _mockProductRepository
            .Setup(r => r.FindFirstSatisfyingAsync(
                It.IsAny<ISpecificationQuery<Product>>()
            ))
            .ReturnsAsync((Product?)null);

        var command = DeactivateProductCommandUtils.CreateCommand();

        await FluentActions
            .Invoking(() => _handler.Handle(command, default))
            .Should()
            .ThrowAsync<ProductNotFoundException>()
            .WithMessage($"Product with id {command.Id} could not be deactivated because it does not exist or is already inactive");

        _mockProductRepository.Verify(r => r.FindFirstSatisfyingAsync(It.IsAny<ISpecificationQuery<Product>>()), Times.Once());
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Never());
    }

    /// <summary>
    /// Tests that when the product exists it is deactivated and the quantity in inventory is set to 0.
    /// </summary>
    [Fact]
    public async Task HandleDeactivateProduct_WhenProductExists_DeactivatesAndSetsInventoryToZero()
    {
        var productToBeDeactivate = ProductUtils.CreateProduct();

        _mockProductRepository
            .Setup(r => r.FindFirstSatisfyingAsync(It.IsAny<QueryActiveProductByIdSpecification>()))
            .ReturnsAsync(productToBeDeactivate);

        var command = DeactivateProductCommandUtils.CreateCommand();

        await _handler.Handle(command, default);

        productToBeDeactivate.IsActive.Should().BeFalse();
        productToBeDeactivate.Inventory.QuantityAvailable.Should().Be(0);
        _mockProductRepository.Verify(r => r.FindFirstSatisfyingAsync(It.IsAny<QueryActiveProductByIdSpecification>()), Times.Once());
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once());
    }
}
