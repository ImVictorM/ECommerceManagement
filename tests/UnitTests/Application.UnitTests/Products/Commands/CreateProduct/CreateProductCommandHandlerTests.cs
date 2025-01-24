using Application.Common.Persistence;
using Application.Products.Commands.CreateProduct;
using Application.UnitTests.Products.Commands.TestUtils;
using Application.UnitTests.TestUtils.Behaviors;

using Domain.ProductAggregate;
using Domain.ProductAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;

using FluentAssertions;
using Microsoft.Extensions.Logging;
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

        _handler = new CreateProductCommandHandler(
            _mockUnitOfWork.Object,
            new Mock<ILogger<CreateProductCommandHandler>>().Object
        );
    }

    /// <summary>
    /// Provides valid <see cref="CreateProductCommand"/> requests with various parameter configurations.
    /// Used for testing the command handler's ability to handle different valid inputs.
    /// </summary>
    public static readonly IEnumerable<object[]> ValidRequests =
    [
        [CreateProductCommandUtils.CreateCommand()],
        [CreateProductCommandUtils.CreateCommand(name: "pencil")],
        [CreateProductCommandUtils.CreateCommand(basePrice: 15m)],
        [CreateProductCommandUtils.CreateCommand(description: "some description")],
        [CreateProductCommandUtils.CreateCommand(initialQuantity: 2)],
        [CreateProductCommandUtils.CreateCommand(categoryIds: NumberUtils.CreateNumberSequenceAsString(2))],
        [CreateProductCommandUtils.CreateCommand(images: ProductUtils.CreateImageURIs(2))],
    ];

    /// <summary>
    /// Verifies that when a valid <see cref="CreateProductCommand"/> is handled by the <see cref="CreateProductCommandHandler"/>,
    /// the product is added to the repository, changes are saved, and a valid product identifier is returned.
    /// </summary>
    /// <param name="request">The valid <see cref="CreateProductCommand"/> request.</param>
    [Theory]
    [MemberData(nameof(ValidRequests))]
    public async Task HandleCreateProduct_WhenCommandIsValid_CreatesAndReturnsTheId(CreateProductCommand request)
    {
        var mockEntityId = ProductId.Create(5);

        MockEFCoreBehaviors.MockSetEntityIdBehavior(_mockProductRepository, _mockUnitOfWork, mockEntityId);

        var actResult = await FluentActions
            .Invoking(() => _handler.Handle(request, default))
            .Should()
            .NotThrowAsync();

        _mockProductRepository.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);

        actResult.Which.Id.Should().NotBeNullOrWhiteSpace();
        actResult.Which.Id.Should().Be(mockEntityId.ToString());
    }
}
