using Application.Categories.Commands.CreateCategory;
using Application.Common.Persistence;
using Application.UnitTests.Categories.Commands.TestUtils;
using Application.UnitTests.TestUtils.Extensions;

using Domain.CategoryAggregate;
using Domain.CategoryAggregate.ValueObjects;

using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.UnitTests.Categories.Commands.CreateCategory;

/// <summary>
/// Unit tests for the <see cref="CreateCategoryCommandHandler"/> command handler.
/// </summary>
public class CreateCategoryCommandHandlerTests
{
    private readonly CreateCategoryCommandHandler _handler;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<ICategoryRepository> _mockCategoryRepository;

    /// <summary>
    /// Initiates a new instance of the <see cref="CreateCategoryCommandHandlerTests"/> class.
    /// </summary>
    public CreateCategoryCommandHandlerTests()
    {
        _mockCategoryRepository = new Mock<ICategoryRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _handler = new CreateCategoryCommandHandler(
            _mockUnitOfWork.Object,
            _mockCategoryRepository.Object,
            new Mock<ILogger<CreateCategoryCommandHandler>>().Object
        );
    }

    /// <summary>
    /// Tests it is possible to create a category with a valid command.
    /// </summary>
    [Fact]
    public async Task HandleCreateCategoryCommand_WithValidCommand_CreatesAndReturnsTheCategoryId()
    {
        var createdId = CategoryId.Create(1);

        var command = CreateCategoryCommandUtils.CreateCommand(name: "home");

        _mockUnitOfWork.MockSetEntityIdBehavior<ICategoryRepository, Category, CategoryId>(
            _mockCategoryRepository,
            createdId
        );

        var result = await _handler.Handle(command, default);

        _mockCategoryRepository.Verify(
            r => r.AddAsync(It.Is<Category>(c => c.Name == command.Name)),
            Times.Once()
        );

        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once());

        result.Id.Should().Be(createdId.ToString());
    }
}
