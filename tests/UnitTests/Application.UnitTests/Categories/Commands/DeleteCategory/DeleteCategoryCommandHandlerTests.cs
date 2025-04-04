using Application.Categories.Commands.DeleteCategory;
using Application.UnitTests.Categories.Commands.TestUtils;
using Application.Common.Persistence.Repositories;
using Application.Common.Persistence;
using Application.Categories.Errors;

using Domain.UnitTests.TestUtils;
using Domain.CategoryAggregate.ValueObjects;
using Domain.CategoryAggregate;

using Microsoft.Extensions.Logging;
using Moq;
using FluentAssertions;

namespace Application.UnitTests.Categories.Commands.DeleteCategory;

/// <summary>
/// Unit tests for the <see cref="DeleteCategoryCommandHandler"/> handler.
/// </summary>
public class DeleteCategoryCommandHandlerTests
{
    private readonly DeleteCategoryCommandHandler _handler;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<ICategoryRepository> _mockCategoryRepository;

    /// <summary>
    /// Initiates a new instance of the
    /// <see cref="DeleteCategoryCommandHandlerTests"/> class.
    /// </summary>
    public DeleteCategoryCommandHandlerTests()
    {
        _mockCategoryRepository = new Mock<ICategoryRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _handler = new DeleteCategoryCommandHandler(
            _mockUnitOfWork.Object,
            _mockCategoryRepository.Object,
            new Mock<ILogger<DeleteCategoryCommandHandler>>().Object
        );
    }

    /// <summary>
    /// Verifies the category is deleted when it exists.
    /// </summary>
    [Fact]
    public async Task HandleDeleteCategoryCommand_WhenCategoryExists_DeletesIt()
    {
        var categoryId = CategoryId.Create(1);
        var category = CategoryUtils.CreateCategory(id: categoryId);
        var command = DeleteCategoryCommandUtils.CreateCommand(
            id: categoryId.ToString()
        );

        _mockCategoryRepository
            .Setup(r => r.FindByIdAsync(
                It.IsAny<CategoryId>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(category);

        await _handler.Handle(command, default);

        _mockCategoryRepository.Verify(
            r => r.RemoveOrDeactivate(category),
            Times.Once()
        );
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once());
    }

    /// <summary>
    /// Verifies  an exception is thrown when the category does not exist.
    /// </summary>
    [Fact]
    public async Task HandleDeleteCategoryCommand_WhenCategoryDoesNotExist_ThrowsError()
    {
        var command = DeleteCategoryCommandUtils.CreateCommand();

        _mockCategoryRepository
            .Setup(r => r.FindByIdAsync(
                It.IsAny<CategoryId>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync((Category?)null);

        await FluentActions
            .Invoking(() => _handler.Handle(command, default))
            .Should()
            .ThrowAsync<CategoryNotFoundException>();

    }
}
