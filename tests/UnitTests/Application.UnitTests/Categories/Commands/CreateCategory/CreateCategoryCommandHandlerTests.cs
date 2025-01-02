using Application.Categories.Commands.CreateCategory;
using Application.Common.Interfaces.Persistence;
using Application.UnitTests.Categories.Commands.TestUtils;
using Application.UnitTests.TestUtils.Behaviors;

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
    private readonly Mock<IRepository<Category, CategoryId>> _mockCategoryRepository;

    /// <summary>
    /// Initiates a new instance of the <see cref="CreateCategoryCommandHandlerTests"/> class.
    /// </summary>
    public CreateCategoryCommandHandlerTests()
    {
        _mockCategoryRepository = new Mock<IRepository<Category, CategoryId>>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _mockUnitOfWork.Setup(uow => uow.CategoryRepository).Returns(_mockCategoryRepository.Object);

        _handler = new CreateCategoryCommandHandler(
            _mockUnitOfWork.Object,
            new Mock<ILogger<CreateCategoryCommandHandler>>().Object
        );
    }

    /// <summary>
    /// Tests it is possible to create a category with a valid command.
    /// </summary>
    [Fact]
    public async Task HandleCreateCategory_WithValidCommand_CreatesCategoryAndReturnsTheCreatedId()
    {
        var createdId = CategoryId.Create(1);
        MockEFCoreBehaviors.MockSetEntityIdBehavior(_mockCategoryRepository, _mockUnitOfWork, createdId);

        var command = CreateCategoryCommandUtils.CreateCommand(name: "home");

        var result = await _handler.Handle(command, default);

        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once());

        result.Id.Should().Be(createdId.ToString());
    }
}
