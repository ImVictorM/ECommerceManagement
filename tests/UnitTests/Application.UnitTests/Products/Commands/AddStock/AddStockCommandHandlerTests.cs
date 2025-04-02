using Application.Common.Persistence;
using Application.Common.Persistence.Repositories;
using Application.Products.Commands.AddStock;
using Application.Products.Errors;
using Application.UnitTests.Products.Commands.TestUtils;

using Domain.ProductAggregate;
using Domain.ProductAggregate.Specifications;
using Domain.UnitTests.TestUtils;

using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.UnitTests.Products.Commands.AddStock;

/// <summary>
/// Unit tests for the <see cref="AddStockCommandHandler"/> handler.
/// </summary>
public class AddStockCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly AddStockCommandHandler _handler;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="AddStockCommandHandlerTests"/> class.
    /// </summary>
    public AddStockCommandHandlerTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockProductRepository = new Mock<IProductRepository>();

        _handler = new AddStockCommandHandler(
            _mockUnitOfWork.Object,
            _mockProductRepository.Object,
            Mock.Of<ILogger<AddStockCommandHandler>>()
        );
    }

    /// <summary>
    /// Verifies that when the product does not exist an exception is thrown.
    /// </summary>
    [Fact]
    public async Task HandleAddStockCommand_WhenProductDoesNotExist_ThrowsError()
    {
        var command = AddStockCommandUtils.CreateCommand();

        _mockProductRepository
            .Setup(r => r.FindFirstSatisfyingAsync(
                It.IsAny<QueryActiveProductByIdSpecification>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync((Product?)null);

        await FluentActions
            .Invoking(() => _handler.Handle(command, default))
            .Should()
            .ThrowAsync<ProductNotFoundException>();
    }

    /// <summary>
    /// Verifies the stock is increased when the product exists.
    /// </summary>
    [Fact]
    public async Task HandleAddStockCommand_WhenProductExists_IncreasesStock()
    {
        var product = ProductUtils.CreateProduct(initialQuantityInInventory: 10);
        var command = AddStockCommandUtils.CreateCommand(quantityToAdd: 22);
        var expectedQuantityAfterIncrement = 32;

        _mockProductRepository
            .Setup(r => r.FindFirstSatisfyingAsync(
                It.IsAny<QueryActiveProductByIdSpecification>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(product);

        await _handler.Handle(command, default);

        product.Inventory.QuantityAvailable
            .Should().Be(expectedQuantityAfterIncrement);
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
    }
}
