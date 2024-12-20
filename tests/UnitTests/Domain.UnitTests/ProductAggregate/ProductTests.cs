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

    /// <summary>
    /// List of valid product parameters.
    /// </summary>
    /// <returns>List of valid product parameters.</returns>
    public static IEnumerable<object[]> ValidProductParameters()
    {
        yield return new object[] {
            DomainConstants.Product.Name,
            DomainConstants.Product.Description,
            DomainConstants.Product.BasePrice,
            DomainConstants.Product.Inventory.QuantityAvailable,
            DomainConstants.Product.Categories,
            DomainConstants.Product.ProductImages
        };

        yield return new object[] {
            "Computer",
            DomainConstants.Product.Description,
            DomainConstants.Product.BasePrice,
            DomainConstants.Product.Inventory.QuantityAvailable,
            DomainConstants.Product.Categories,
            ProductUtils.CreateProductImages(2),
        };

        yield return new object[] {
            DomainConstants.Product.Name,
            "Some description for the product",
            DomainConstants.Product.BasePrice,
            DomainConstants.Product.Inventory.QuantityAvailable,
            DomainConstants.Product.Categories,
            ProductUtils.CreateProductImages(3),
        };

        yield return new object[] {
            DomainConstants.Product.Name,
            DomainConstants.Product.Description,
            100m,
            DomainConstants.Product.Inventory.QuantityAvailable,
            DomainConstants.Product.Categories,
            ProductUtils.CreateProductImages(4),
        };

        yield return new object[] {
            DomainConstants.Product.Name,
            DomainConstants.Product.Description,
            DomainConstants.Product.BasePrice,
            57,
            DomainConstants.Product.Categories,
            ProductUtils.CreateProductImages(5),
        };

        yield return new object[] {
            DomainConstants.Product.Name,
            DomainConstants.Product.Description,
            DomainConstants.Product.BasePrice,
            DomainConstants.Product.Inventory.QuantityAvailable,
            ProductUtils.CreateProductCategories(2),
            ProductUtils.CreateProductImages(6),
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
    /// <param name="images">The product image URLs.</param>
    [Theory]
    [MemberData(nameof(ValidProductParameters))]
    public void Product_WhenCreatingWithValidParameter_ReturnsNewInstance(
        string name,
        string description,
        decimal price,
        int quantityAvailable,
        IEnumerable<ProductCategory> categories,
        IEnumerable<ProductImage> images
    )
    {
        var act = () =>
        {
            var product = ProductUtils.CreateProduct(
               name: name,
               description: description,
               price: price,
               quantityAvailable: quantityAvailable,
               categories: categories,
               images: images
            );

            product.Should().NotBeNull();
            product.Name.Should().Be(name);
            product.Description.Should().Be(description);
            product.BasePrice.Should().Be(price);
            product.Inventory.QuantityAvailable.Should().Be(quantityAvailable);
            product.ProductImages.Should().BeEquivalentTo(images);
            product.ProductCategories.Should().BeEquivalentTo(categories);
        };

        act.Should().NotThrow();
    }


    /// <summary>
    /// Tests it is possible to update a product correctly.
    /// </summary>
    [Fact]
    public void Product_WhenUpdatingProduct_UpdatesCorrectly()
    {
        var product = ProductUtils.CreateProduct();

        var newName = "new name";
        var newDescription = "new description";
        var newPrice = 200m;
        var newImages = ProductUtils.CreateProductImages(4);
        var newCategories = ProductUtils.CreateProductCategories(2);

        product.UpdateProduct(
            name: newName,
            description: newDescription,
            basePrice: newPrice,
            images: newImages,
            categories: newCategories
        );

        product.Name.Should().Be(newName);
        product.Description.Should().Be(newDescription);
        product.BasePrice.Should().Be(newPrice);
        product.ProductImages.Should().BeEquivalentTo(newImages);
        product.ProductCategories.Should().BeEquivalentTo(newCategories);
    }

    /// <summary>
    /// Tests that is possible to increment the product's inventory quantity available.
    /// </summary>
    [Theory]
    [InlineData(10, 55, 65)]
    [InlineData(20, 5, 25)]
    [InlineData(12, 1, 13)]
    public void Product_WhenAddingProductsToInventory_IncrementsInventoryQuantityAvailable(
        int initialQuantity,
        int quantityToAdd,
        int expectedQuantityAvailable
    )
    {
        var product = ProductUtils.CreateProduct(quantityAvailable: initialQuantity);

        product.IncrementQuantityInInventory(quantityToAdd);

        product.Inventory.QuantityAvailable.Should().Be(expectedQuantityAvailable);
    }

    /// <summary>
    /// Tests that making a product inactivate deactivates it and sets the inventory to 0.
    /// </summary>
    [Fact]
    public void Product_WhenMakingInactive_DeactivatesAndSetsInventoryToZero()
    {
        var product = ProductUtils.CreateProduct(quantityAvailable: 500);

        product.MakeInactive();

        product.IsActive.Should().BeFalse();
        product.Inventory.QuantityAvailable.Should().Be(0);
    }
}
