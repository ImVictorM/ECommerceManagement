using Application.Common.Persistence.Repositories;
using Application.Categories.Errors;
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
    private readonly Mock<ICategoryRepository> _mockCategoryRepository;

    /// <summary>
    /// Initiates a new instance of the
    /// <see cref="GetCategoryByIdQueryHandlerTests"/> class.
    /// </summary>
    public GetCategoryByIdQueryHandlerTests()
    {
        _mockCategoryRepository = new Mock<ICategoryRepository>();

        _handler = new GetCategoryByIdQueryHandler(
            _mockCategoryRepository.Object,
            new Mock<ILogger<GetCategoryByIdQueryHandler>>().Object
        );
    }

    /// <summary>
    /// Verifies an exception is thrown when the category does not exist.
    /// </summary>
    [Fact]
    public async Task HandleGetCategoryByIdQuery_WhenCategoryDoesNotExist_ThrowsError()
    {
        var query = GetCategoryByIdQueryUtils.CreateQuery();

        _mockCategoryRepository
            .Setup(r => r.FindByIdAsync(
                It.IsAny<CategoryId>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync((Category?)null);

        await FluentActions
            .Invoking(() => _handler.Handle(query, default))
            .Should()
            .ThrowAsync<CategoryNotFoundException>();
    }

    /// <summary>
    /// Verifies the category is returned when it exists.
    /// </summary>
    [Fact]
    public async Task HandleGetCategoryByIdQuery_WhenCategoryExists_ReturnsIt()
    {
        var categoryId = CategoryId.Create(1);
        var category = CategoryUtils.CreateCategory(id: categoryId);

        var query = GetCategoryByIdQueryUtils.CreateQuery(
            id: categoryId.ToString()
        );

        _mockCategoryRepository
            .Setup(r => r.FindByIdAsync(
                It.IsAny<CategoryId>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(category);

        var result = await _handler.Handle(query, default);

        result.Id.Should().Be(category.Id.ToString());
        result.Name.Should().Be(category.Name);
    }
}
