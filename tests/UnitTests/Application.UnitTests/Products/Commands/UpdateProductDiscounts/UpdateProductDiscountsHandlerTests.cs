using Application.Common.Interfaces.Persistence;
using Application.Products.Commands.UpdateProductDiscounts;
using Application.Products.Queries.Common.Errors;
using Application.UnitTests.Products.Commands.TestUtils;
using Domain.ProductAggregate;
using Domain.ProductAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using SharedKernel.Interfaces;
using SharedKernel.UnitTests.TestUtils;
using SharedKernel.ValueObjects;

namespace Application.UnitTests.Products.Commands.UpdateProductDiscounts;

/// <summary>
/// Unit tests for the process of updating a product's discount list.
/// </summary>
public class UpdateProductDiscountsHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRepository<Product, ProductId>> _mockProductRepository;
    private readonly UpdateProductDiscountsCommandHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateProductDiscountsHandlerTests"/> class,
    /// </summary>
    public UpdateProductDiscountsHandlerTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockProductRepository = new Mock<IRepository<Product, ProductId>>();

        _mockUnitOfWork.Setup(uow => uow.ProductRepository).Returns(_mockProductRepository.Object);

        _handler = new UpdateProductDiscountsCommandHandler(
            _mockUnitOfWork.Object,
            new Mock<ILogger<UpdateProductDiscountsCommandHandler>>().Object
        );
    }

    /// <summary>
    /// Tests that when the product does not exist an exception is thrown.
    /// </summary>
    [Fact]
    public async Task HandleUpdateProductDiscounts_WhenProductDoesNotExist_ThrowsError()
    {
        var command = UpdateProductDiscountsCommandUtils.CreateCommand();

        _mockProductRepository
            .Setup(r => r.FindByIdSatisfyingAsync(It.IsAny<ProductId>(), It.IsAny<ISpecificationQuery<Product>>()))
            .ReturnsAsync((Product?)null);

        await FluentActions
            .Invoking(() => _handler.Handle(command, default))
        .Should()
        .ThrowAsync<ProductNotFoundException>()
            .WithMessage($"Product with id {command.Id} does not exist");

        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Never());
    }

    /// <summary>
    /// Tests that when the product exists the discounts are updated.
    /// </summary>
    [Fact]
    public async Task HandleUpdateProductDiscounts_WhenProductExists_UpdatesTheDiscounts()
    {
        var now = DateTimeOffset.UtcNow;

        var applicableDiscounts = new List<Discount>()
        {
            DiscountUtils.CreateDiscount(percentage: 10, startingDate: now, endingDate: now.AddHours(10)),
            DiscountUtils.CreateDiscount(percentage: 20, startingDate: now, endingDate: now.AddMonths(2))
        };
        var futureDiscounts = new List<Discount>()
        {
            DiscountUtils.CreateDiscount(percentage: 5, startingDate: now.AddDays(4), endingDate: now.AddDays(6) ),
            DiscountUtils.CreateDiscount(percentage: 2, startingDate: now.AddMonths(1), endingDate: now.AddMonths(2))
        };

        var discountsToAdd = applicableDiscounts.Concat(futureDiscounts);

        var product = ProductUtils.CreateProduct(initialDiscounts: []);
        var command = UpdateProductDiscountsCommandUtils.CreateCommand(discounts: discountsToAdd);

        _mockProductRepository
            .Setup(r => r.FindByIdSatisfyingAsync(It.IsAny<ProductId>(), It.IsAny<ISpecificationQuery<Product>>()))
            .ReturnsAsync(product);

        await _handler.Handle(command, default);

        product.Discounts.Should().BeEquivalentTo(discountsToAdd);
        product.GetApplicableDiscounts().Should().BeEquivalentTo(applicableDiscounts);

        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once());
    }
}
