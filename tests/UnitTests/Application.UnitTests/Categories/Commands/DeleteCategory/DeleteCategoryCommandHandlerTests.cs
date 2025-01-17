using Application.Categories.Commands.DeleteCategory;
using Application.UnitTests.Categories.Commands.TestUtils;
using Application.Categories.Common.Errors;

using Domain.UnitTests.TestUtils;
using Domain.CategoryAggregate.ValueObjects;
using Domain.CategoryAggregate;

using Microsoft.Extensions.Logging;
using Moq;
using FluentAssertions;
using Application.Common.Persistence;

namespace Application.UnitTests.Categories.Commands.DeleteCategory;

/// <summary>
/// Unit tests for the <see cref="DeleteCategoryCommandHandler"/> handler.
/// </summary>
public class DeleteCategoryCommandHandlerTests
{
    private readonly DeleteCategoryCommandHandler _handler;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRepository<Category, CategoryId>> _mockCategoryRepository;

    /// <summary>
    /// Initiates a new instance of the <see cref="DeleteCategoryCommandHandlerTests"/> class.
    /// </summary>
    public DeleteCategoryCommandHandlerTests()
    {
        _mockCategoryRepository = new Mock<IRepository<Category, CategoryId>>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _mockUnitOfWork.Setup(uow => uow.CategoryRepository).Returns(_mockCategoryRepository.Object);

        _handler = new DeleteCategoryCommandHandler(
            _mockUnitOfWork.Object,
            new Mock<ILogger<DeleteCategoryCommandHandler>>().Object
        );
    }

    /// <summary>
    /// Tests when the category exists it is deleted.
    /// </summary>
    [Fact]
    public async Task HandleDeleteCategory_WhenCategoryExists_DeletesIt()
    {
        var categoryId = CategoryId.Create(1);
        var category = CategoryUtils.CreateCategory(id: categoryId);
        var command = DeleteCategoryCommandUtils.CreateCommand(id: categoryId.ToString());

        _mockCategoryRepository
            .Setup(r => r.FindByIdAsync(It.IsAny<CategoryId>()))
            .ReturnsAsync(category);

        await _handler.Handle(command, default);

        _mockCategoryRepository.Verify(r => r.RemoveOrDeactivate(category), Times.Once());
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once());
    }

    /// <summary>
    /// Tests when the category does not exist an exception is thrown.
    /// </summary>
    [Fact]
    public async Task HandleDeleteCategory_WhenCategoryDoesNotExist_ThrowsError()
    {
        var command = DeleteCategoryCommandUtils.CreateCommand();

        _mockCategoryRepository
            .Setup(r => r.FindByIdAsync(It.IsAny<CategoryId>()))
            .ReturnsAsync((Category?)null);

        await FluentActions
            .Invoking(() => _handler.Handle(command, default))
            .Should()
            .ThrowAsync<CategoryNotFoundException>();

    }
}
