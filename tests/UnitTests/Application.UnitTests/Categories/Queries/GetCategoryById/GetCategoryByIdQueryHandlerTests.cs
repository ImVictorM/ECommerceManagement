using Application.Common.Interfaces.Persistence;
using Application.Categories.Common.Errors;
using Application.Categories.Queries.GetCategoryById;
using Application.UnitTests.Categories.Queries.TestUtils;

using Domain.CategoryAggregate.ValueObjects;
using Domain.CategoryAggregate;

using Microsoft.Extensions.Logging;
using Moq;
using FluentAssertions;

using Domain.UnitTests.TestUtils;

namespace Application.UnitTests.Categories.Queries.GetCategoryById;

/// <summary>
/// Unit tests for the <see cref="GetCategoryByIdQueryHandler"/> handler.
/// </summary>
public class GetCategoryByIdQueryHandlerTests
{
    private readonly GetCategoryByIdQueryHandler _handler;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRepository<Category, CategoryId>> _mockCategoryRepository;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetCategoryByIdQueryHandlerTests"/> class.
    /// </summary>
    public GetCategoryByIdQueryHandlerTests()
    {
        _mockCategoryRepository = new Mock<IRepository<Category, CategoryId>>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _mockUnitOfWork.Setup(uow => uow.CategoryRepository).Returns(_mockCategoryRepository.Object);

        _handler = new GetCategoryByIdQueryHandler(
            _mockUnitOfWork.Object,
            new Mock<ILogger<GetCategoryByIdQueryHandler>>().Object
        );
    }

    /// <summary>
    /// Tests an exception is thrown when the category does not exist.
    /// </summary>
    [Fact]
    public async Task HandleGetCategoryById_WhenCategoryDoesNotExist_ThrowsError()
    {
        _mockCategoryRepository
            .Setup(r => r.FindByIdAsync(It.IsAny<CategoryId>()))
            .ReturnsAsync((Category?)null);

        var query = GetCategoryByIdQueryUtils.CreateQuery();

        await FluentActions
            .Invoking(() => _handler.Handle(query, default))
            .Should()
            .ThrowAsync<CategoryNotFoundException>();
    }

    /// <summary>
    /// Tests when the category exists it is returned.
    /// </summary>
    [Fact]
    public async Task HandleGetCategoryById_WhenCategoryExists_RetrievesIt()
    {
        var categoryId = CategoryId.Create(1);
        var category = CategoryUtils.CreateCategory(id: categoryId);

        _mockCategoryRepository
            .Setup(r => r.FindByIdAsync(It.IsAny<CategoryId>()))
            .ReturnsAsync(category);

        var query = GetCategoryByIdQueryUtils.CreateQuery(id: categoryId.ToString());

        var result = await _handler.Handle(query, default);

        result.Id.Should().Be(category.Id.ToString());
        result.Name.Should().Be(category.Name);
    }
}
