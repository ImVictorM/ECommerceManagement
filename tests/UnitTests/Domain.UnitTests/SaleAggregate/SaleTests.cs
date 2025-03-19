using Domain.SaleAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;
using Domain.CategoryAggregate.ValueObjects;
using Domain.ProductAggregate.ValueObjects;
using Domain.SaleAggregate;
using Domain.SaleAggregate.Errors;

using SharedKernel.UnitTests.TestUtils;
using SharedKernel.ValueObjects;

using FluentAssertions;

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
            new HashSet<SaleCategory>
            {
                SaleCategory.Create(CategoryId.Create(3))
            },
            new HashSet<SaleProduct>
            {
                SaleProduct.Create(ProductId.Create(5))
            },
            new HashSet<SaleProduct>
            {
                SaleProduct.Create(ProductId.Create(6))
            }
        ]
    ];
    /// <summary>
    /// Ensures that a sale is created successfully with valid inputs.
    /// </summary>
    [Theory]
    [MemberData(nameof(ValidSaleInputs))]
    public void CreateSale_WithValidInputs_CreatesWithoutThrowing(
        Discount discount,
        HashSet<SaleCategory> categoriesOnSale,
        HashSet<SaleProduct> productsOnSale,
        HashSet<SaleProduct> productsExcludedFromSale
    )
    {
        var act = FluentActions
            .Invoking(() => SaleUtils.CreateSale(
                discount: discount,
                categoriesOnSale: categoriesOnSale,
                productsOnSale: productsOnSale,
                productsExcludedFromSale:
                productsExcludedFromSale
            ))
            .Should()
            .NotThrow();

        var sale = act.Subject;

        sale.Discount.Should().BeEquivalentTo(discount);
        sale.CategoriesOnSale.Should().BeEquivalentTo(categoriesOnSale);
        sale.ProductsOnSale.Should().BeEquivalentTo(productsOnSale);
        sale.ProductsExcludedFromSale.Should().BeEquivalentTo(productsExcludedFromSale);
    }

    /// <summary>
    /// Ensures that creating a sale with no categories or products throws an exception.
    /// </summary>
    [Fact]
    public void CreateSale_WithoutCategoriesOrProducts_ThrowsError()
    {
        var emptyCategories = new HashSet<SaleCategory>();
        var emptyProducts = new HashSet<SaleProduct>();

        FluentActions
            .Invoking(() => SaleUtils.CreateSale(
                categoriesOnSale: emptyCategories,
                productsOnSale: emptyProducts
            ))
            .Should()
            .Throw<InvalidSaleStateException>()
            .WithMessage(
                "A sale must contain at least one category or one product"
            );
    }

    /// <summary>
    /// Ensures that creating a sale with no categories and products excluded equal
    /// to the products included throws an exception.
    /// </summary>
    [Fact]
    public void CreateSale_WithoutCategoriesAndProductsOnSaleEqualToProductsExcluded_ThrowsError()
    {
        var emptyCategories = new HashSet<SaleCategory>();
        var productsOnSale = new HashSet<SaleProduct>()
        {
            SaleProduct.Create(ProductId.Create(1)),
            SaleProduct.Create(ProductId.Create(2)),
        };
        var productsExcluded = new HashSet<SaleProduct>()
        {
            SaleProduct.Create(ProductId.Create(1)),
            SaleProduct.Create(ProductId.Create(2)),
        };

        FluentActions
            .Invoking(() => SaleUtils.CreateSale(
                categoriesOnSale: emptyCategories,
                productsOnSale: productsOnSale,
                productsExcludedFromSale: productsExcluded
            ))
            .Should()
            .Throw<InvalidSaleStateException>()
            .WithMessage(
                "A sale must contain at least one category or one product"
            );
    }

    /// <summary>
    /// Provides test data for validating if a product is in a sale.
    /// </summary>
    public static IEnumerable<object[]> IsProductOnSaleData =>
    [
        [
            SaleEligibleProduct.Create(
                ProductId.Create(1),
                [
                    CategoryId.Create(1)
                ]
            ),
            true
        ],
        [
            SaleEligibleProduct.Create(
                ProductId.Create(3),
                [
                    CategoryId.Create(1)
                ]
            ),
            true
        ],
        [
            SaleEligibleProduct.Create(
                ProductId.Create(2),
                [
                    CategoryId.Create(1)
                ]
            ),
            false
        ],
        [
            SaleEligibleProduct.Create(
                ProductId.Create(3),
                [
                    CategoryId.Create(3)
                ]
            ),
            false
        ],
    ];

    /// <summary>
    /// Validates whether a product is on sale correctly.
    /// </summary>
    /// <param name="product">The product to check.</param>
    /// <param name="expectedResult">The expected result.</param>
    [Theory]
    [MemberData(nameof(IsProductOnSaleData))]
    public void IsProductOnSale_WithDifferentEligibleProducts_ReturnExpectedResult(
        SaleEligibleProduct product,
        bool expectedResult
    )
    {
        var sale = SaleUtils.CreateSale(
            productsOnSale:
            [
                SaleProduct.Create(ProductId.Create(1)),
                SaleProduct.Create(ProductId.Create(2))
            ],
            categoriesOnSale:
            [
                SaleCategory.Create(CategoryId.Create(1)),
            ],
            productsExcludedFromSale:
            [
                SaleProduct.Create(ProductId.Create(2))
            ]
        );

        var result = sale.IsProductOnSale(product);

        result.Should().Be(expectedResult);
    }

    /// <summary>
    /// Validates the product is not on sale if the sale discount is not valid
    /// to date.
    /// </summary>
    [Fact]
    public void IsProductOnSale_WithDiscountNotValidToDate_ReturnFalse()
    {
        var product = SaleEligibleProduct.Create(
            ProductId.Create(1),
            [
                CategoryId.Create(2)
            ]
        );

        var sale = SaleUtils.CreateSale(
            productsOnSale:
            [
                SaleProduct.Create(product.ProductId),
            ],
            discount: DiscountUtils.CreateDiscount(
                startingDate: DateTimeOffset.UtcNow.AddDays(4),
                endingDate: DateTimeOffset.UtcNow.AddDays(15)
            )
        );

        var result = sale.IsProductOnSale(product);

        result.Should().BeFalse();
    }
}
