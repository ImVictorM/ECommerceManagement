using Application.Products.Queries.GetProductCategories;
using Application.UnitTests.Products.Queries.TestUtils;
using Domain.ProductAggregate.Enumerations;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.UnitTests.Products.Queries.GetProductCategories;

/// <summary>
/// Unit tests for the <see cref="GetProductCategoriesQueryHandler"/> query handler.
/// </summary>
public class GetProductCategoriesQueryHandlerTests
{
    private readonly GetProductCategoriesQueryHandler _handler;

    /// <summary>
    /// Initiates a new instance of the <see cref="GetProductCategoriesQueryHandlerTests"/> class.
    /// </summary>
    public GetProductCategoriesQueryHandlerTests()
    {
        _handler = new GetProductCategoriesQueryHandler(new Mock<ILogger<GetProductCategoriesQueryHandler>>().Object);
    }

    /// <summary>
    /// Tests that the handler returns all the categories correctly.
    /// </summary>
    [Fact]
    public async Task HandleGetProductCategories_WhenQueryIsHandled_ReturnsAllCategories()
    {
        var query = GetProductCategoriesQueryUtils.CreateQuery();

        var result = await _handler.Handle(query, default);

        result.Categories.Should().BeEquivalentTo(Category.List());
    }
}
