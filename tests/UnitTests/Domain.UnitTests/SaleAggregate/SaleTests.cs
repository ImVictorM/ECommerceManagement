using Domain.SaleAggregate.ValueObjects;
using FluentAssertions;
using SharedKernel.Errors;
using SharedKernel.UnitTests.TestUtils;
using Domain.UnitTests.TestUtils;
using Domain.CategoryAggregate.ValueObjects;
using Domain.ProductAggregate.ValueObjects;
using Domain.UnitTests.TestUtils.Constants;
using Domain.SaleAggregate;
using SharedKernel.ValueObjects;

namespace Domain.UnitTests.SaleAggregate;

/// <summary>
/// Unit tests for the <see cref="Sale"/> class.
/// </summary>
public class SaleTests
{
    /// <summary>
    /// List containing valid sale inputs to create a sale.
    /// </summary>
    public static readonly IEnumerable<object[]> ValidSaleInputs =
    [
        [
            DiscountUtils.CreateDiscount(PercentageUtils.Create(15)),
            new HashSet<CategoryReference>
            {
                CategoryReference.Create(CategoryId.Create(3))
            },
            new HashSet<ProductReference>
            {
                ProductReference.Create(ProductId.Create(5))
            },
            new HashSet<ProductReference>
            {
                ProductReference.Create(ProductId.Create(6))
            }
        ]
    ];
    /// <summary>
    /// Ensures that a sale is created successfully with valid inputs using custom values.
    /// </summary>
    [Theory]
    [MemberData(nameof(ValidSaleInputs))]
    public void CreateSale_WithValidInputs_ShouldCreateSaleWithExpectedValues(
        Discount discount,
        HashSet<CategoryReference> categoriesInSale,
        HashSet<ProductReference> productsInSale,
        HashSet<ProductReference> productsExcludedFromSale
    )
    {
        var sale = SaleUtils.CreateSale(discount, categoriesInSale, productsInSale, productsExcludedFromSale);

        sale.Discount.Should().BeEquivalentTo(discount);
        sale.CategoriesInSale.Should().BeEquivalentTo(categoriesInSale);
        sale.ProductsInSale.Should().BeEquivalentTo(productsInSale);
        sale.ProductsExcludedFromSale.Should().BeEquivalentTo(productsExcludedFromSale);
    }

    /// <summary>
    /// Ensures that creating a sale with no categories or products throws an exception.
    /// </summary>
    [Fact]
    public void CreateSale_WithNoCategoriesOrProducts_ShouldThrowDomainValidationException()
    {
        var emptyCategories = new HashSet<CategoryReference>();
        var emptyProducts = new HashSet<ProductReference>();

        var act = () => SaleUtils.CreateSale(categoriesInSale: emptyCategories, productsInSale: emptyProducts);

        act.Should().Throw<DomainValidationException>()
           .WithMessage("A sale must contain at least one category or one product.");
    }

    /// <summary>
    /// Ensures that creating a sale with no categories and products excluded equal to the products included throws an exception.
    /// </summary>
    [Fact]
    public void CreateSale_WithNoCategoriesAndProductsExcludedAreEqualToProductsIncluded_ShouldThrowDomainValidationException()
    {
        var emptyCategories = new HashSet<CategoryReference>();
        var productsIncluded = new HashSet<ProductReference>()
        {
            ProductReference.Create(ProductId.Create(1)),
            ProductReference.Create(ProductId.Create(2)),
        };
        var productsExcluded = new HashSet<ProductReference>()
        {
            ProductReference.Create(ProductId.Create(1)),
            ProductReference.Create(ProductId.Create(2)),
        };

        var act = () => SaleUtils.CreateSale(
            categoriesInSale: emptyCategories,
            productsInSale: productsIncluded,
            productsExcludeFromSale: productsExcluded
        );

        act.Should().Throw<DomainValidationException>()
           .WithMessage("A sale must contain at least one category or one product.");
    }

    /// <summary>
    /// Ensures that a sale with a valid discount is considered valid for the current date.
    /// </summary>
    [Fact]
    public void IsValidToDate_WithFutureValidDiscount_ShouldReturnTrue()
    {
        var discount = DiscountUtils.CreateDiscount(
            PercentageUtils.Create(DomainConstants.Sale.DiscountPercentage),
            startingDate: DateTimeOffset.UtcNow.AddSeconds(-200),
            endingDate: DateTimeOffset.UtcNow.AddDays(2)
        );

        var sale = SaleUtils.CreateSale(discount);

        var isValid = sale.IsValidToDate();

        isValid.Should().BeTrue();
    }

    /// <summary>
    /// Ensures that a sale with an expired discount is considered invalid for the current date.
    /// </summary>
    [Fact]
    public void IsValidToDate_WithExpiredDiscount_ShouldReturnFalse()
    {
        // Arrange
        var discount = DiscountUtils.CreateDiscount(
            PercentageUtils.Create(DomainConstants.Sale.DiscountPercentage),
            startingDate: DateTimeOffset.UtcNow.AddDays(2),
            endingDate: DateTimeOffset.UtcNow.AddDays(5)
        );
        var sale = SaleUtils.CreateSale(discount);

        var isValid = sale.IsValidToDate();

        isValid.Should().BeFalse();
    }

    /// <summary>
    /// Provides test data for validating if a product is in a sale.
    /// </summary>
    public static IEnumerable<object[]> IsProductInSaleData =>
    [
        [
            SaleProduct.Create(
                ProductId.Create(1),
                new HashSet<CategoryId>()
                {
                    CategoryId.Create(1)
                }
            ),
            true
        ],
        [
            SaleProduct.Create(
                ProductId.Create(3),
                new HashSet<CategoryId>()
                {
                    CategoryId.Create(1)
                }
            ),
            true
        ],
        [
            SaleProduct.Create(
                ProductId.Create(2),
                new HashSet<CategoryId>()
                {
                    CategoryId.Create(1)
                }
            ),
            false
        ],
        [
            SaleProduct.Create(
                ProductId.Create(3),
                new HashSet<CategoryId>()
                {
                    CategoryId.Create(3)
                }
            ),
            false
        ],
    ];

    /// <summary>
    /// Validates whether a product is correctly identified as being in a sale.
    /// </summary>
    /// <param name="product">The product to check.</param>
    /// <param name="expectedResult">The expected result.</param>
    [Theory]
    [MemberData(nameof(IsProductInSaleData))]
    public void IsProductInSale_WhenChecked_ShouldReturnExpectedResult(SaleProduct product, bool expectedResult)
    {
        var sale = SaleUtils.CreateSale(
            productsInSale: new HashSet<ProductReference>()
            {
                ProductReference.Create(ProductId.Create(1)),
                ProductReference.Create(ProductId.Create(2))
            },
            categoriesInSale: new HashSet<CategoryReference>()
            {
                CategoryReference.Create(CategoryId.Create(1)),
            },
            productsExcludeFromSale: new HashSet<ProductReference>()
            {
                ProductReference.Create(ProductId.Create(2))
            }
        );

        var result = sale.IsProductInSale(product);

        result.Should().Be(expectedResult);
    }
}
