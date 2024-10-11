using Domain.ProductAggregate.ValueObjects;
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
            TestConstants.Product.Category,
            ProductUtils.CreateProductImagesUrl(1),
        };

        yield return new object[] {
            "Computer",
            TestConstants.Product.Description,
            TestConstants.Product.Price,
            TestConstants.Product.QuantityAvailable,
            TestConstants.Product.Category,
            ProductUtils.CreateProductImagesUrl(2),
        };

        yield return new object[] {
            TestConstants.Product.Name,
            "Some description for the product",
            TestConstants.Product.Price,
            TestConstants.Product.QuantityAvailable,
            TestConstants.Product.Category,
            ProductUtils.CreateProductImagesUrl(3),
        };

        yield return new object[] {
            TestConstants.Product.Name,
            TestConstants.Product.Description,
            100m,
            TestConstants.Product.QuantityAvailable,
            TestConstants.Product.Category,
            ProductUtils.CreateProductImagesUrl(4),
        };

        yield return new object[] {
            TestConstants.Product.Name,
            TestConstants.Product.Description,
            TestConstants.Product.Price,
            57,
            TestConstants.Product.Category,
            ProductUtils.CreateProductImagesUrl(5),
        };

        yield return new object[] {
            TestConstants.Product.Name,
            TestConstants.Product.Description,
            TestConstants.Product.Price,
            TestConstants.Product.QuantityAvailable,
            ProductCategory.Automotive,
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
    /// <param name="category">The product category.</param>
    /// <param name="productImageUrls">The product image urls.</param>
    [Theory]
    [MemberData(nameof(ValidProductParameters))]
    public void Product_WhenCreatingWithValidParameter_ReturnsNewInstance(
        string name,
        string description,
        decimal price,
        int quantityAvailable,
        ProductCategory category,
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
                category,
                productImageUrls
            );

            product.Should().NotBeNull();
            product.Name.Should().Be(name);
            product.Description.Should().Be(description);
            product.Price.Should().Be(price);
            product.Inventory.QuantityAvailable.Should().Be(quantityAvailable);
            product.ProductCategory.Should().Be(category);

            for (var i = 0; i < product.ProductImages.Count; i += 1)
            {
                var image = product.ProductImages[i];
                image.Url.Should().Be($"{TestConstants.Product.ProductImage}-{i}");
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
}
