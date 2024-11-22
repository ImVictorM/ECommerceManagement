using Application.Common.Interfaces.Persistence;
using Application.Products.Queries.Common.Errors;
using Application.Products.Queries.GetProductById;
using Application.UnitTests.Products.Queries.TestUtils;
using Domain.ProductAggregate;
using Domain.ProductAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;
using FluentAssertions;
using Moq;
using SharedKernel.Interfaces;

namespace Application.UnitTests.Products.Queries.GetProductById;

/// <summary>
/// Unit tests for the <see cref="GetProductByIdQueryHandler"/> handler.
/// </summary>
public class GetProductByIdQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRepository<Product, ProductId>> _mockProductRepository;
    private readonly GetProductByIdQueryHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetProductByIdQueryHandlerTests"/> class,
    /// </summary>
    public GetProductByIdQueryHandlerTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockProductRepository = new Mock<IRepository<Product, ProductId>>();

        _mockUnitOfWork.Setup(uow => uow.ProductRepository).Returns(_mockProductRepository.Object);

        _handler = new GetProductByIdQueryHandler(_mockUnitOfWork.Object);
    }

    /// <summary>
    /// Tests that when the product being retrieved exists it is returned.
    /// </summary>
    [Fact]
    public async Task HandleGetProductById_WhenProductExists_ReturnsIt()
    {
        var query = GetProductByIdQueryUtils.CreateQuery(id: "1");
        var productToFind = ProductUtils.CreateProduct();

        _mockProductRepository
            .Setup(r => r.FindByIdSatisfyingAsync(It.IsAny<ProductId>(), It.IsAny<ISpecificationQuery<Product>>()))
            .ReturnsAsync(productToFind);

        var result = await _handler.Handle(query, default);

        result.Product.Should().BeEquivalentTo(productToFind);
    }

    /// <summary>
    /// Tests that when the product being retrieved does not exist throws a not found error.
    /// </summary>
    [Fact]
    public async Task HandleGetProductById_WhenProductDoesNotExistOrIsInactive_ThrowsNotFoundError()
    {
        var notFoundId = "5";
        var query = GetProductByIdQueryUtils.CreateQuery(id: notFoundId);

        _mockProductRepository
            .Setup(r => r.FindByIdSatisfyingAsync(It.IsAny<ProductId>(), It.IsAny<ISpecificationQuery<Product>>()))
            .ReturnsAsync((Product?)null!);

        await FluentActions
            .Invoking(() => _handler.Handle(query, default))
            .Should()
            .ThrowAsync<ProductNotFoundException>()
            .WithMessage($"The product with id {notFoundId} does not exist");
    }
}
