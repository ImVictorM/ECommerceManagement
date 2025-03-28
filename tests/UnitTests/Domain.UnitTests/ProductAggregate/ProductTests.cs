using Domain.CategoryAggregate.ValueObjects;
using Domain.ProductAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;
using Domain.ProductAggregate;

using FluentAssertions;

namespace Domain.UnitTests.ProductAggregate;

/// <summary>
/// Unit tests for the <see cref="Product"/> class.
/// </summary>
public class ProductTests
{

    /// <summary>
    /// Provides a list of valid actions to create new products.
    /// </summary>
    public static readonly IEnumerable<object[]> ValidActionsToCreateProducts =
    [
        [
            () => ProductUtils.CreateProduct(name: "Computer")
        ],
        [
            () => ProductUtils.CreateProduct(
                description: "Some description for the product"
            )
        ],
        [
            () => ProductUtils.CreateProduct(basePrice: 100m)
        ],
        [
            () => ProductUtils.CreateProduct(initialQuantityInInventory: 57)
        ],
        [
            () => ProductUtils.CreateProduct(categories:
                [
                    ProductCategory.Create(CategoryId.Create(1)),
                    ProductCategory.Create(CategoryId.Create(2))
                ]
            )
        ],
        [
            () => ProductUtils.CreateProduct(images:
                [
                    ProductImage.Create(
                        new Uri("product-image.png", UriKind.Relative)
                    )
                ]
            )
        ],
    ];

    /// <summary>
    /// Verifies it is possible to create a new product with valid parameters.
    /// </summary>
    [Theory]
    [MemberData(nameof(ValidActionsToCreateProducts))]
    public void Create_WithValidParameters_CreatesWithoutThrowing(
        Func<Product> action
    )
    {
        var actionResult = FluentActions
            .Invoking(action)
            .Should()
            .NotThrow();

        var product = actionResult.Subject;

        product.Should().NotBeNull();
        product.Name.Should().NotBeNullOrWhiteSpace();
        product.Description.Should().NotBeNullOrWhiteSpace();
        product.BasePrice.Should().BePositive();
        product.Inventory.QuantityAvailable.Should().BePositive();
        product.ProductImages.Count.Should().BeGreaterThanOrEqualTo(1);
        product.ProductCategories.Count.Should().BeGreaterThanOrEqualTo(1);
    }

    /// <summary>
    /// Verifies it is possible to update a product correctly.
    /// </summary>
    [Fact]
    public void Update_WithValidParameters_UpdatesCorrectly()
    {
        var product = ProductUtils.CreateProduct();

        var newName = "new name";
        var newDescription = "new description";
        var newPrice = 200m;
        var newImages = new List<ProductImage>()
        {
            ProductImage.Create(new Uri("image-1.png", UriKind.Relative)),
            ProductImage.Create(new Uri("image-2.png", UriKind.Relative))
        };
        var newCategories = new List<ProductCategory>()
        {
            ProductCategory.Create(CategoryId.Create(1)),
            ProductCategory.Create(CategoryId.Create(2))
        };

        product.Update(
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
    /// Verifies that making a product inactivate deactivates it and sets the
    /// inventory to 0.
    /// </summary>
    [Fact]
    public void Deactivate_WithActiveProduct_DeactivatesItAndClearsInventory()
    {
        var product = ProductUtils.CreateProduct(initialQuantityInInventory: 500);

        product.Deactivate();

        product.IsActive.Should().BeFalse();
        product.Inventory.QuantityAvailable.Should().Be(0);
    }
}
