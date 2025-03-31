using Application.Common.Persistence;
using Application.Common.Persistence.Repositories;
using Application.Products.Commands.CreateProduct;
using Application.UnitTests.Products.Commands.TestUtils;
using Application.UnitTests.TestUtils.Extensions;

using Domain.ProductAggregate;
using Domain.ProductAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;

using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.UnitTests.Products.Commands.CreateProduct;

/// <summary>
/// Unit tests for <see cref="CreateProductCommandHandler"/> command handler.
/// </summary>
public class CreateProductCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly CreateProductCommandHandler _handler;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="CreateProductCommandHandlerTests"/> class.
    /// </summary>
    public CreateProductCommandHandlerTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockProductRepository = new Mock<IProductRepository>();

        _handler = new CreateProductCommandHandler(
            _mockUnitOfWork.Object,
            _mockProductRepository.Object,
            Mock.Of<ILogger<CreateProductCommandHandler>>()
        );
    }

    /// <summary>
    /// Provides valid <see cref="CreateProductCommand"/> requests.
    /// </summary>
    public static readonly IEnumerable<object[]> ValidRequests =
    [
        [
            CreateProductCommandUtils.CreateCommand()
        ],
        [
            CreateProductCommandUtils.CreateCommand(name: "pencil")
        ],
        [
                CreateProductCommandUtils.CreateCommand(basePrice: 15m)
        ],
        [
            CreateProductCommandUtils.CreateCommand(description: "some description")
        ],
        [
            CreateProductCommandUtils.CreateCommand(initialQuantity: 2)
        ],
        [
            CreateProductCommandUtils.CreateCommand(
                categoryIds: NumberUtils.CreateNumberSequenceAsString(2)
            )
        ],
        [
            CreateProductCommandUtils.CreateCommand(
                images: ProductUtils.CreateImageURIs(2)
            )
        ],
    ];

    /// <summary>
    /// Verifies a product is created correct with a valid requests.
    /// </summary>
    /// <param name="request">The valid request.</param>
    [Theory]
    [MemberData(nameof(ValidRequests))]
    public async Task HandleCreateProductCommand_WithValidRequest_ReturnsCreatedResult(
        CreateProductCommand request
    )
    {
        var mockEntityId = ProductId.Create(5);

        _mockUnitOfWork.MockSetEntityIdBehavior
            <IProductRepository, Product, ProductId>(
                _mockProductRepository,
                mockEntityId
            );

        var actResult = await FluentActions
            .Invoking(() => _handler.Handle(request, default))
            .Should()
            .NotThrowAsync();

        _mockProductRepository.Verify(
            r => r.AddAsync(It.IsAny<Product>()),
            Times.Once
        );
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);

        actResult.Which.Id.Should().NotBeNullOrWhiteSpace();
        actResult.Which.Id.Should().Be(mockEntityId.ToString());
    }
}
