using Application.Common.Interfaces.Persistence;
using Application.Categories.Queries.GetCategories;
using Application.UnitTests.Categories.Queries.TestUtils;

using Domain.CategoryAggregate.ValueObjects;
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
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRepository<Category, CategoryId>> _mockCategoryRepository;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetCategoriesQueryHandlerTests"/> class.
    /// </summary>
    public GetCategoriesQueryHandlerTests()
    {
        _mockCategoryRepository = new Mock<IRepository<Category, CategoryId>>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _mockUnitOfWork.Setup(uow => uow.CategoryRepository).Returns(_mockCategoryRepository.Object);

        _handler = new GetCategoriesQueryHandler(
            _mockUnitOfWork.Object,
            new Mock<ILogger<GetCategoriesQueryHandler>>().Object
        );
    }

    /// <summary>
    /// Tests all the categories are retrieved when the handler is called.
    /// </summary>
    [Fact]
    public async Task HandleGetCategories_WhenCalled_ReturnsAllTheCategories()
    {
        var categories = CategoryUtils.CreateCategories(4).ToList();

        _mockCategoryRepository
            .Setup(r => r.FindAllAsync(It.IsAny<Expression<Func<Category, bool>>>()))
            .ReturnsAsync(categories);

        var query = GetCategoriesQueryUtils.CreateQuery();

        var result = await _handler.Handle(query, default);

        foreach (var (categoryResult, category) in result.Zip(categories))
        {
            categoryResult.Id.Should().Be(category.Id.ToString());
            categoryResult.Name.Should().Be(category.Name);
        }
    }
}
