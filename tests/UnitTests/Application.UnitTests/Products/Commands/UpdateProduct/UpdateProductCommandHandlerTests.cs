using Application.Common.Persistence;
using Application.Common.Persistence.Repositories;
using Application.Products.Commands.UpdateProduct;
using Application.Products.Errors;
using Application.UnitTests.Products.Commands.TestUtils;

using Domain.ProductAggregate;
using Domain.ProductAggregate.Specifications;
using Domain.UnitTests.TestUtils;

using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.UnitTests.Products.Commands.UpdateProduct;

/// <summary>
/// Unit tests for the <see cref="UpdateProductCommandHandler"/> handler.
/// </summary>
public class UpdateProductCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly UpdateProductCommandHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateProductCommandHandlerTests"/> class,
    /// </summary>
    public UpdateProductCommandHandlerTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockProductRepository = new Mock<IProductRepository>();

        _handler = new UpdateProductCommandHandler(
            _mockUnitOfWork.Object,
            _mockProductRepository.Object,
            new Mock<ILogger<UpdateProductCommandHandler>>().Object
        );
    }

    /// <summary>
    /// Tests when the product does not exists an exception is thrown.
    /// </summary>
    [Fact]
    public async Task HandleUpdateProduct_WhenProductDoesNotExist_ThrowsError()
    {
        var notFoundId = "14";
        var command = UpdateProductCommandUtils.CreateCommand(id: notFoundId);

        _mockProductRepository
            .Setup(r => r.FindFirstSatisfyingAsync(
                It.IsAny<QueryActiveProductByIdSpecification>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync((Product?)null);

        await FluentActions
            .Invoking(() => _handler.Handle(command, default))
            .Should()
            .ThrowAsync<ProductNotFoundException>()
            .WithMessage($"The product with id {notFoundId} could not be updated because it does not exist");
    }

    /// <summary>
    /// Tests when the product exists it is updated correctly.
    /// </summary>
    [Fact]
    public async Task HandleUpdateProduct_WhenProductExists_UpdatesIt()
    {
        var productToBeUpdated = ProductUtils.CreateProduct();

        var command = UpdateProductCommandUtils.CreateCommand(
            name: "new product name",
            description: "new product description",
            basePrice: 600m,
            images: [new Uri("new-image.png", UriKind.Relative)],
            categoryIds: ["1", "2", "3"]
        );

        _mockProductRepository
            .Setup(r => r.FindFirstSatisfyingAsync(
                It.IsAny<QueryActiveProductByIdSpecification>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(productToBeUpdated);

        await _handler.Handle(command, default);

        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once());
        productToBeUpdated.Name.Should().Be(command.Name);
        productToBeUpdated.Description.Should().Be(command.Description);
        productToBeUpdated.BasePrice.Should().Be(command.BasePrice);
        productToBeUpdated.ProductCategories.Select(pc => pc.CategoryId.ToString()).Should().BeEquivalentTo(command.CategoryIds);
        productToBeUpdated.ProductImages.Select(pi => pi.Url).Should().BeEquivalentTo(command.Images);
    }
}
