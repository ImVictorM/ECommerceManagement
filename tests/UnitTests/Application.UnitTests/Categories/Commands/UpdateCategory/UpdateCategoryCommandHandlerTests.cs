using Application.UnitTests.Categories.Commands.TestUtils;
using Application.Categories.Commands.UpdateCategory;
using Application.Common.Persistence.Repositories;
using Application.Common.Persistence;
using Application.Categories.Errors;

using Domain.CategoryAggregate.ValueObjects;
using Domain.CategoryAggregate;
using Domain.UnitTests.TestUtils;

using Microsoft.Extensions.Logging;
using Moq;
using FluentAssertions;

namespace Application.UnitTests.Categories.Commands.UpdateCategory;

/// <summary>
/// Unit tests for the <see cref="UpdateCategoryCommandHandler"/> handler.
/// </summary>
public class UpdateCategoryCommandHandlerTests
{
    private readonly UpdateCategoryCommandHandler _handler;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<ICategoryRepository> _mockCategoryRepository;

    /// <summary>
    /// Initiates a new instance of the
    /// <see cref="UpdateCategoryCommandHandlerTests"/> class.
    /// </summary>
    public UpdateCategoryCommandHandlerTests()
    {
        _mockCategoryRepository = new Mock<ICategoryRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _handler = new UpdateCategoryCommandHandler(
            _mockUnitOfWork.Object,
            _mockCategoryRepository.Object,
            new Mock<ILogger<UpdateCategoryCommandHandler>>().Object
        );
    }

    /// <summary>
    /// Verifies an exception is thrown when the category does not exist.
    /// </summary>
    [Fact]
    public async Task HandleUpdateCategoryCommand_WhenCategoryDoesNotExist_ThrowsError()
    {
        _mockCategoryRepository
            .Setup(r => r.FindByIdAsync(
                It.IsAny<CategoryId>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync((Category?)null);

        var request = UpdateCategoryCommandUtils.CreateCommand();

        await FluentActions
            .Invoking(() => _handler.Handle(request, default))
            .Should()
            .ThrowAsync<CategoryNotFoundException>();
    }

    /// <summary>
    /// Verifies the category is updated when it exists.
    /// </summary>
    [Fact]
    public async Task HandleUpdateCategoryCommand_WhenCategoryExists_UpdatesIt()
    {
        var categoryId = CategoryId.Create(1);
        var category = CategoryUtils.CreateCategory(id: categoryId);

        var command = UpdateCategoryCommandUtils.CreateCommand(
            id: categoryId.ToString(),
            name: "music"
        );

        _mockCategoryRepository
            .Setup(r => r.FindByIdAsync(
                It.IsAny<CategoryId>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(category);

        await _handler.Handle(command, default);

        category.Name.Should().Be(command.Name);

        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once());
    }
}
