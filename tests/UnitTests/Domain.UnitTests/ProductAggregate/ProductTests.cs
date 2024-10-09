using Domain.UnitTests.TestUtils;
using Domain.UnitTests.TestUtils.Constants;
using FluentAssertions;

namespace Domain.UnitTests.ProductAggregate;

/// <summary>
/// Tests for the <see cref="Domain.ProductAggregate.Product"/> aggregate root.
/// </summary>
public class ProductTests
{

    public static IEnumerable<object[]> ValidProductParameters()
    {
        yield return new object[] {
            TestConstants.Product.Name,
            TestConstants.Product.Description,
            TestConstants.Product.Price,
            TestConstants.Product.QuantityAvailable,
            ProductUtils.CreateProductImagesUrl(1),
        };

        yield return new object[] {
            "Computer",
            TestConstants.Product.Description,
            TestConstants.Product.Price,
            TestConstants.Product.QuantityAvailable,
            ProductUtils.CreateProductImagesUrl(2),
        };

        yield return new object[] {
            TestConstants.Product.Name,
            "Some description for the product",
            TestConstants.Product.Price,
            TestConstants.Product.QuantityAvailable,
            ProductUtils.CreateProductImagesUrl(3),
        };

        yield return new object[] {
            TestConstants.Product.Name,
            TestConstants.Product.Description,
            100m,
            TestConstants.Product.QuantityAvailable,
            ProductUtils.CreateProductImagesUrl(4),
        };

        yield return new object[] {
            TestConstants.Product.Name,
            TestConstants.Product.Description,
            TestConstants.Product.Price,
            57,
            ProductUtils.CreateProductImagesUrl(5),
        };

        yield return new object[] {
            TestConstants.Product.Name,
            TestConstants.Product.Description,
            TestConstants.Product.Price,
            TestConstants.Product.QuantityAvailable,
            ProductUtils.CreateProductImagesUrl(6),
        };
    }

    /// <summary>
    /// Tests if it is possible to create a new product with valid parameters.
    /// </summary>
    /// <param name="name">The product name.</param>
    /// <param name="description">The product description.</param>
    /// <param name="price">The product price.</param>
    /// <param name="quantityAvailable">The product quantity available.</param>
    /// <param name="productImageUrls">The product image urls.</param>
    [Theory]
    [MemberData(nameof(ValidProductParameters))]
    public void Product_WhenCreatingWithValidParameter_ReturnsNewInstance(
        string name,
        string description,
        decimal price,
        int quantityAvailable,
        IEnumerable<Uri> productImageUrls
    )
    {
        var act = () =>
        {
            var product = ProductUtils.CreateProduct(
                ProductCategoryUtils.CreateProductCategoryId(),
                name,
                description,
                price,
                quantityAvailable,
                productImageUrls
            );

            product.Should().NotBeNull();
            product.Name.Should().Be(name);
            product.Description.Should().Be(description);
            product.Price.Should().Be(price);
            product.Inventory.QuantityAvailable.Should().Be(quantityAvailable);

            for (var i = 0; i < product.ProductImages.Count; i += 1)
            {
                var image = product.ProductImages[i];
                image.Url.Should().Be($"{TestConstants.Product.ProductImage}-{i}");
            }
        };

        act.Should().NotThrow();
    }

    /// <summary>
    /// Tests if it is possible to add a discount to the product by discount id.
    /// </summary>
    [Fact]
    public void Product_WhenRelatingItWithDiscount_AddsAndIncrementsToTheDiscountList()
    {
        var discountId = 5;
        var product = ProductUtils.CreateProduct();

        product.AddDiscount(DiscountUtils.CreateDiscountId(discountId));

        product.ProductDiscounts.Should().NotBeNull();
        product.ProductDiscounts.Count.Should().Be(1);
        product.ProductDiscounts.Select(pd => pd.DiscountId.Value).Should().Contain(discountId);
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
}
