using Domain.ProductAggregate.Enumerations;
using Domain.UnitTests.TestUtils;
using Domain.UnitTests.TestUtils.Constants;
using FluentAssertions;
using SharedKernel.UnitTests.TestUtils;
using SharedKernel.ValueObjects;

namespace Domain.UnitTests.ProductAggregate;

/// <summary>
/// Tests for the <see cref="Domain.ProductAggregate.Product"/> aggregate root.
/// </summary>
public class ProductTests
{

    /// <summary>
    /// List of valid product parameters.
    /// </summary>
    /// <returns>List of valid product parameters.</returns>
    public static IEnumerable<object[]> ValidProductParameters()
    {
        yield return new object[] {
            DomainConstants.Product.Name,
            DomainConstants.Product.Description,
            DomainConstants.Product.Price,
            DomainConstants.Product.QuantityAvailable,
            DomainConstants.Product.Categories,
            ProductUtils.CreateProductImagesUrl(1),
        };

        yield return new object[] {
            "Computer",
            DomainConstants.Product.Description,
            DomainConstants.Product.Price,
            DomainConstants.Product.QuantityAvailable,
            DomainConstants.Product.Categories,
            ProductUtils.CreateProductImagesUrl(2),
        };

        yield return new object[] {
            DomainConstants.Product.Name,
            "Some description for the product",
            DomainConstants.Product.Price,
            DomainConstants.Product.QuantityAvailable,
            DomainConstants.Product.Categories,
            ProductUtils.CreateProductImagesUrl(3),
        };

        yield return new object[] {
            DomainConstants.Product.Name,
            DomainConstants.Product.Description,
            100m,
            DomainConstants.Product.QuantityAvailable,
            DomainConstants.Product.Categories,
            ProductUtils.CreateProductImagesUrl(4),
        };

        yield return new object[] {
            DomainConstants.Product.Name,
            DomainConstants.Product.Description,
            DomainConstants.Product.Price,
            57,
            DomainConstants.Product.Categories,
            ProductUtils.CreateProductImagesUrl(5),
        };

        yield return new object[] {
            DomainConstants.Product.Name,
            DomainConstants.Product.Description,
            DomainConstants.Product.Price,
            DomainConstants.Product.QuantityAvailable,
            ProductUtils.GetCategoryNames(Category.Footwear),
            ProductUtils.CreateProductImagesUrl(6),
        };
    }

    /// <summary>
    /// A list containing the product price, a list of discounts, and the expected price after those discounts.
    /// </summary>
    public static IEnumerable<object[]> PriceWithDiscountsAndExpectedPriceAfterDiscounts()
    {
        yield return new object[]
        {
            100,
            new List<Discount>() {
                DiscountUtils.CreateDiscount(percentage: 20),
                DiscountUtils.CreateDiscount(percentage: 10)
            },
            72
        };

        yield return new object[]
        {
            200,
            new List<Discount>() {
                DiscountUtils.CreateDiscount(percentage: 10)
            },
            180
        };
    }
    /// <summary>
    /// Tests if it is possible to create a new product with valid parameters.
    /// </summary>
    /// <param name="name">The product name.</param>
    /// <param name="description">The product description.</param>
    /// <param name="price">The product price.</param>
    /// <param name="quantityAvailable">The product quantity available.</param>
    /// <param name="categories">The product categories.</param>
    /// <param name="productImageUrls">The product image urls.</param>
    [Theory]
    [MemberData(nameof(ValidProductParameters))]
    public void Product_WhenCreatingWithValidParameter_ReturnsNewInstance(
        string name,
        string description,
        decimal price,
        int quantityAvailable,
        IEnumerable<string> categories,
        IEnumerable<Uri> productImageUrls
    )
    {
        var act = () =>
        {
            var product = ProductUtils.CreateProduct(
                name,
                description,
                price,
                quantityAvailable,
                categories,
                productImageUrls
            );

            product.Should().NotBeNull();
            product.Name.Should().Be(name);
            product.Description.Should().Be(description);
            product.BasePrice.Should().Be(price);
            product.Inventory.QuantityAvailable.Should().Be(quantityAvailable);
            product.GetCategoryNames().Should().BeEquivalentTo(categories);

            for (var i = 0; i < product.ProductImages.Count; i += 1)
            {
                var image = product.ProductImages[i];
                image.Url.Should().Be($"{DomainConstants.Product.ProductImage}-{i}");
            }
        };

        act.Should().NotThrow();
    }

    /// <summary>
    /// Tests if it is possible to make a product inactive.
    /// </summary>
    [Fact]
    public void Product_WhenMakingItInactive_SetsTheIsActiveFieldToFalse()
    {
        var product = ProductUtils.CreateProduct();

        product.MakeInactive();

        product.IsActive.Should().BeFalse();
    }

    /// <summary>
    /// Tests the correctness of adding a discount to a product.
    /// </summary>
    [Fact]
    public void Product_WhenAddingDiscount_AddsAndIncrementTheProductDiscounts()
    {
        var product = ProductUtils.CreateProduct();

        var tenPercentDiscount = DiscountUtils.CreateDiscount(percentage: 10);
        var fivePercentDiscount = DiscountUtils.CreateDiscount(percentage: 5);

        product.AddDiscounts(tenPercentDiscount, fivePercentDiscount);

        product.Discounts.Should().NotBeEmpty();
        product.Discounts.Count.Should().Be(2);
        product.Discounts.Should().Contain(tenPercentDiscount);
        product.Discounts.Should().Contain(fivePercentDiscount);
    }

    /// <summary>
    /// Tests getting the price after discounts calculates the price with discount correctly.
    /// </summary>
    /// <param name="price">The product price.</param>
    /// <param name="discounts">The product discounts.</param>
    /// <param name="expectedPriceAfterDiscount">The expected price after discounts were applied.</param>
    [Theory]
    [MemberData(nameof(PriceWithDiscountsAndExpectedPriceAfterDiscounts))]
    public void Product_WhenGettingPriceAfterDiscount_CalculatesItCorrectly(
        decimal price,
        IEnumerable<Discount> discounts,
        decimal expectedPriceAfterDiscount
    )
    {
        var product = ProductUtils.CreateProduct(
            price: price,
            initialDiscounts: discounts
        );

        product.GetPriceAfterDiscounts().Should().Be(expectedPriceAfterDiscount);
    }
}
