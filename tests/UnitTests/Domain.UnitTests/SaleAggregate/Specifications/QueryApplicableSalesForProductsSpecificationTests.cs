using Domain.CategoryAggregate.ValueObjects;
using Domain.ProductAggregate.ValueObjects;
using Domain.SaleAggregate.Specifications;
using Domain.SaleAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;

using SharedKernel.UnitTests.TestUtils;

using FluentAssertions;

namespace Domain.UnitTests.SaleAggregate.Specifications;

/// <summary>
/// Unit tests for the <see cref="QueryApplicableSalesForProductsSpecification"/>
/// class.
/// </summary>
public class QueryApplicableSalesForProductsSpecificationTests
{
    /// <summary>
    /// Verifies that the specification returns true when a product on sale
    /// matches the query.
    /// </summary>
    [Fact]
    public void QueryApplicableSalesForProducts_WithMatchingProduct_ReturnsTrue()
    {
        var productId = ProductId.Create(1);

        var sale = SaleUtils.CreateSale(
            productsOnSale:
            [
                SaleProduct.Create(productId)
            ]
        );

        var specification = new QueryApplicableSalesForProductsSpecification(
        [
            SaleEligibleProduct.Create(productId, [])
        ]);

        var result = specification.Criteria.Compile()(sale);

        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies that the specification returns true when a category in the
    /// sale matches the query.
    /// </summary>
    [Fact]
    public void QueryApplicableSalesForProducts_WithMatchingCategory_ReturnsTrue()
    {
        var categoryId = CategoryId.Create(1);

        var sale = SaleUtils.CreateSale(
            categoriesOnSale: [SaleCategory.Create(categoryId)]
        );

        var specification = new QueryApplicableSalesForProductsSpecification(
        [
            SaleEligibleProduct.Create(ProductId.Create(1), [categoryId])
        ]);

        var result = specification.Criteria.Compile()(sale);

        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies that the specification returns false when the product is
    /// excluded from the sale.
    /// </summary>
    [Fact]
    public void QueryApplicableSalesForProducts_WhenProductIsExcluded_ReturnsFalse()
    {
        var productId = ProductId.Create(1);
        var sale = SaleUtils.CreateSale(
            productsExcludedFromSale: [SaleProduct.Create(productId)]
        );

        var specification = new QueryApplicableSalesForProductsSpecification(
        [
            SaleEligibleProduct.Create(productId, [])
        ]);

        var result = specification.Criteria.Compile()(sale);

        result.Should().BeFalse();
    }

    /// <summary>
    /// Verifies that the specification returns false when the sale is not valid
    /// to date.
    /// </summary>
    [Fact]
    public void QueryApplicableSalesForProducts_WhenSaleIsNotValidToDate_ReturnsFalse()
    {
        var productId = ProductId.Create(1);
        var expiredDiscount = DiscountUtils.CreateDiscount(
            startingDate: DateTimeOffset.UtcNow.AddDays(3),
            endingDate: DateTimeOffset.UtcNow.AddDays(15)
        );

        var sale = SaleUtils.CreateSale(
            productsOnSale: [SaleProduct.Create(productId)],
            discount: expiredDiscount
        );

        var specification = new QueryApplicableSalesForProductsSpecification(
        [
            SaleEligibleProduct.Create(productId, [])
        ]);

        var result = specification.Criteria.Compile()(sale);

        result.Should().BeFalse();
    }

    /// <summary>
    /// Verifies that the specification returns true when the sale is valid
    /// to date.
    /// </summary>
    [Fact]
    public void QueryApplicableSalesForProducts_WhenSaleIsValidToDate_ReturnsTrue()
    {
        var productId = ProductId.Create(1);

        var sale = SaleUtils.CreateSale(
            productsOnSale: [SaleProduct.Create(productId)],
            discount: DiscountUtils.CreateDiscountValidToDate()
        );

        var specification = new QueryApplicableSalesForProductsSpecification(
        [
            SaleEligibleProduct.Create(productId, [])
        ]);

        var result = specification.Criteria.Compile()(sale);

        result.Should().BeTrue();
    }
}
