using Application.Common.Interfaces.Persistence;
using Application.Products.Commands.CreateProduct;
using Application.UnitTests.Products.Commands.TestUtils;
using Domain.ProductAggregate;
using Domain.ProductAggregate.Enumerations;
using Domain.ProductAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Products.Commands.CreateProduct;

/// <summary>
/// Unit tests for <see cref="CreateProductCommandHandler"/>, verifying the correct behavior when handling the creation of a product.
/// </summary>
public class CreateProductCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRepository<Product, ProductId>> _mockProductRepository;
    private readonly CreateProductCommandHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateProductCommandHandlerTests"/> class,
    /// </summary>
    public CreateProductCommandHandlerTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockProductRepository = new Mock<IRepository<Product, ProductId>>();

        _mockUnitOfWork.Setup(uow => uow.ProductRepository).Returns(_mockProductRepository.Object);

        _handler = new CreateProductCommandHandler(_mockUnitOfWork.Object);
    }

    /// <summary>
    /// Provides valid <see cref="CreateProductCommand"/> requests with various parameter configurations.
    /// Used for testing the command handler's ability to handle different valid inputs.
    /// </summary>
    /// <returns>A collection of valid <see cref="CreateProductCommand"/> requests.</returns>
    public static IEnumerable<object[]> ValidRequests()
    {
        yield return new object[] { CreateProductCommandUtils.CreateCommand() };
        yield return new object[] { CreateProductCommandUtils.CreateCommand(name: "pencil") };
        yield return new object[] { CreateProductCommandUtils.CreateCommand(initialPrice: 15m) };
        yield return new object[] { CreateProductCommandUtils.CreateCommand(description: "some description") };
        yield return new object[] { CreateProductCommandUtils.CreateCommand(initialQuantity: 2) };
        yield return new object[] { CreateProductCommandUtils.CreateCommand(categories: ProductUtils.GetCategoryNames(Category.Automotive, Category.Beauty)) };
        yield return new object[] { CreateProductCommandUtils.CreateCommand(images: ProductUtils.CreateProductImagesUrl(5)) };
        yield return new object[] { CreateProductCommandUtils.CreateCommand(initialDiscounts: DiscountUtils.CreateDiscounts(2)) };
    }

    /// <summary>
    /// Verifies that when a valid <see cref="CreateProductCommand"/> is handled by the <see cref="CreateProductCommandHandler"/>,
    /// the product is added to the repository, changes are saved, and a valid product identifier is returned.
    /// </summary>
    /// <param name="request">The valid <see cref="CreateProductCommand"/> request.</param>
    [Theory]
    [MemberData(nameof(ValidRequests))]
    public async Task HandleCreateProduct_WhenCommandIsValid_AddsThenSavesThenReturnsTheId(CreateProductCommand request)
    {
        var actResult = await FluentActions
            .Invoking(() => _handler.Handle(request, default))
            .Should()
            .NotThrowAsync();

        _mockProductRepository.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);

        actResult.Which.Id.Should().NotBeNullOrWhiteSpace();
    }
}
