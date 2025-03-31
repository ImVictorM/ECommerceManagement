using Application.Common.Persistence.Repositories;
using Application.Categories.Queries.GetCategories;
using Application.UnitTests.Categories.Queries.TestUtils;

using Domain.CategoryAggregate;
using Domain.UnitTests.TestUtils;

using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;
using FluentAssertions;

namespace Application.UnitTests.Categories.Queries.GetCategories;

/// <summary>
/// Unit tests for the <see cref="GetCategoriesQueryHandler"/> handler.
/// </summary>
public class GetCategoriesQueryHandlerTests
{
    private readonly GetCategoriesQueryHandler _handler;
    private readonly Mock<ICategoryRepository> _mockCategoryRepository;

    /// <summary>
    /// Initiates a new instance of the
    /// <see cref="GetCategoriesQueryHandlerTests"/> class.
    /// </summary>
    public GetCategoriesQueryHandlerTests()
    {
        _mockCategoryRepository = new Mock<ICategoryRepository>();

        _handler = new GetCategoriesQueryHandler(
            _mockCategoryRepository.Object,
            new Mock<ILogger<GetCategoriesQueryHandler>>().Object
        );
    }

    /// <summary>
    /// Verifies all the categories are retrieved.
    /// </summary>
    [Fact]
    public async Task HandleGetCategories_WithValidRequest_ReturnsAllTheCategories()
    {
        var categories = CategoryUtils.CreateCategories(4).ToList();

        var query = GetCategoriesQueryUtils.CreateQuery();

        _mockCategoryRepository
            .Setup(r => r.FindAllAsync(
                It.IsAny<Expression<Func<Category, bool>>>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(categories);

        var result = await _handler.Handle(query, default);

        foreach (var (categoryResult, category) in result.Zip(categories))
        {
            categoryResult.Id.Should().Be(category.Id.ToString());
            categoryResult.Name.Should().Be(category.Name);
        }
    }
}
