using Domain.CategoryAggregate.ValueObjects;
using Domain.ProductAggregate.Specifications;
using Domain.ProductAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;
using FluentAssertions;

namespace Domain.UnitTests.ProductAggregate.Specifications;

/// <summary>
/// Unit tests for the <see cref="QueryProductsContainingCategoriesSpecification"/> class.
/// </summary>
public class QueryProductsContainingCategoriesSpecificationTests
{
    /// <summary>
    /// Verifies that the specification returns true when the product contains all the categories specified.
    /// </summary>
    [Fact]
    public void QueryProductsContainingCategoriesSpecification_WhenCategoriesMatch_ReturnsTrue()
    {
        var product = ProductUtils.CreateProduct(categories: [
            ProductCategory.Create(CategoryId.Create(1)),
            ProductCategory.Create(CategoryId.Create(2)),
        ]);

        var specification = new QueryProductsContainingCategoriesSpecification(
        [
            ProductCategory.Create(CategoryId.Create(1)),
            ProductCategory.Create(CategoryId.Create(2)),
        ]);

        var result = specification.Criteria.Compile()(product);

        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies that the specification returns false when the product does not contain all the categories specified.
    /// </summary>
    [Fact]
    public void QueryProductsContainingCategoriesSpecification_WhenCategoriesDoesNotMatch_ReturnsFalse()
    {
        var product = ProductUtils.CreateProduct(categories: [
            ProductCategory.Create(CategoryId.Create(1)),
        ]);

        var specification = new QueryProductsContainingCategoriesSpecification(
        [
            ProductCategory.Create(CategoryId.Create(1)),
            ProductCategory.Create(CategoryId.Create(2)),
        ]);

        var result = specification.Criteria.Compile()(product);

        result.Should().BeFalse();
    }
}
