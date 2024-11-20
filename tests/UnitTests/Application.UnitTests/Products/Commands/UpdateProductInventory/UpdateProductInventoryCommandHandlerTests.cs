using Application.Common.Interfaces.Persistence;
using Application.Products.Commands.UpdateProductInventory;
using Application.Products.Queries.Common.Errors;
using Application.UnitTests.Products.Commands.TestUtils;
using Domain.ProductAggregate;
using Domain.ProductAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Products.Commands.UpdateProductInventory;

/// <summary>
/// Unit tests for the <see cref="UpdateProductInventoryCommandHandler"/> handler.
/// </summary>
public class UpdateProductInventoryCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRepository<Product, ProductId>> _mockProductRepository;
    private readonly UpdateProductInventoryCommandHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateProductInventoryCommandHandlerTests"/> class,
    /// </summary>
    public UpdateProductInventoryCommandHandlerTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockProductRepository = new Mock<IRepository<Product, ProductId>>();

        _mockUnitOfWork.Setup(uow => uow.ProductRepository).Returns(_mockProductRepository.Object);

        _handler = new UpdateProductInventoryCommandHandler(_mockUnitOfWork.Object);
    }

    /// <summary>
    /// Tests that when the product does not exist an exception is thrown.
    /// </summary>
    [Fact]
    public async Task HandleUpdateProductInventory_WhenProductDoesNotExist_ThrowsError()
    {
        var command = UpdateProductInventoryCommandUtils.CreateCommand();

        _mockProductRepository
            .Setup(r => r.FindByIdAsync(It.IsAny<ProductId>()))
            .ReturnsAsync((Product?)null);

        await FluentActions
            .Invoking(() => _handler.Handle(command, default))
            .Should()
            .ThrowAsync<ProductNotFoundException>()
            .WithMessage($"It was not possible to increment the inventory of the product with id {command.ProductId} because the product does not exist");

        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Never());
    }

    /// <summary>
    /// Tests that when the product exists the quantity available in inventory is incremented.
    /// </summary>
    [Fact]
    public async Task HandleUpdateProductInventory_WhenProductExists_UpdatesTheInventoryQuantity()
    {
        var product = ProductUtils.CreateProduct(quantityAvailable: 10);
        var command = UpdateProductInventoryCommandUtils.CreateCommand(quantityToAdd: 22);
        var expectedQuantityAfterIncrement = 32;

        _mockProductRepository
            .Setup(r => r.FindByIdAsync(It.IsAny<ProductId>()))
            .ReturnsAsync(product);

        await _handler.Handle(command, default);

        product.Inventory.QuantityAvailable.Should().Be(expectedQuantityAfterIncrement);
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once());
    }
}
